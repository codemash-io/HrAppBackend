using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HrApp
{
    public class MenuRepository : IMenuRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<string> InsertMenu(Menu menu)
        {
            if (menu == null)
            {
                throw  new ArgumentNullException(nameof(menu), "Cannot insert menu in db, because menu is empty");
            }
            
            var repo = new CodeMashRepository<MenuEntity>(Client);

            var entity = new MenuEntity
            {
                DivisionId = menu.Division?.Id,
                Employees = menu.Employees.Select(x => x.Id).ToList(),
                PlannedDate = menu.LunchDate
            };
                
            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response.Id;
        }

        public async Task UpdateMenuLunchTime(DateTime newTime, Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);
            
            await repo.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Set(x => x.PlannedDate, newTime), null);
        }

        public async Task MakeEmployeeOrder(Menu menu, List<PersonalOrderPreference> preferences, EmployeeEntity employeeEntity)
        {
            PersonalOrderPreference FindPreference(FoodType foodType)
            {
                return  preferences.FirstOrDefault(x => x.Type == foodType);
            }

            // TODO: clear all the preferences
            var service = new CodeMashRepository<MenuEntity>(Client);

            var mainCourse = FindPreference(FoodType.Main);
            
            if (mainCourse != null)
            {
                //var arrayFilter = Builders<BsonDocument>.Filter.Eq("main_dish_options.items.no", mainCourse.FoodNumber)
                //& Builders<BsonDocument>.Filter.Eq("scores.type", "quiz");                

                    var update = Builders<MenuEntity>.Update.Set("planned_lunch_date", DateTime.Now);
                    await service.UpdateOneAsync(x => x.Id == menu.Id, update, null);

                // await service.UpdateOneAsync(x => x.Id == menu.Id,
                // Builders<MenuEntity>.Update.AddToSet($"main_dish_options.items.no[{mainCourse.FoodNumber}].employees", employeeEntity.Id), null);
            }
            
            var soup = FindPreference(FoodType.Soup);

            if (soup != null)
            {
                await service.UpdateOneAsync(x => x.Id == menu.Id,
                    Builders<MenuEntity>.Update.AddToSet($"soups.items[{soup.FoodNumber}].employees", employeeEntity.Id), null);
            }

            var drink = FindPreference(FoodType.Drinks);

            if (drink != null)
            {
                await service.UpdateOneAsync(x => x.Id == menu.Id,
                    Builders<MenuEntity>.Update.AddToSet($"drinks.items[{drink.FoodNumber}].employees", employeeEntity.Id), null);
            }

            var souce = FindPreference(FoodType.Soup);

            if (souce != null)
            {
                await service.UpdateOneAsync(x => x.Id == menu.Id,
                    Builders<MenuEntity>.Update.AddToSet($"souces.items[{souce.FoodNumber}].employees", employeeEntity.Id), null);
            }
            // TODO: add extra

        }

        public async Task AdjustMenuStatus(Menu menu, MenuStatus status)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);
            
            await repo.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Set(x => x.Status, status.ToString()), null);

        }

        async Task<List<Guid>> IMenuRepository.GetEmployeesWhoOrderedFood(Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);

            var menuEntity = await repo.FindOneAsync(x => x.Id == menu.Id);
            //var menuEntity = response;
            var employees = GetEmployeesList(menuEntity);
            var employeesGuid = GetEmployeesGuid(employees);

            return employeesGuid.Select(Guid.Parse).ToList();
        }

        async Task<List<Guid>> IMenuRepository.GetEmployeesWhoStillNotMadeAnOrder(Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);

            var menuEntity = await repo.FindOneAsync(x => x.Id == menu.Id);          
            var employees = GetEmployeesList(menuEntity);        
            var employeeswoOrders = menuEntity.Employees.Except(employees).ToList();
            var employeesGuid = GetEmployeesGuid(employeeswoOrders);

            return employeesGuid.Select(Guid.Parse).ToList();
        }

        protected List<string> GetEmployeesList(MenuEntity menuEntity)
        {
            var employees = new List<string>();
            menuEntity.MainFood.ForEach(x => x.Employees.Where(d => !employees.Any(c => c == d)).ToList()
   .ForEach(f => employees.Add(f)));
            menuEntity.Soup.ForEach(x => x.Employees.Where(d => !employees.Any(c => c == d)).ToList()
    .ForEach(f => employees.Add(f)));
            menuEntity.Drink.ForEach(x => x.Employees.Where(d => !employees.Any(c => c == d)).ToList()
    .ForEach(f => employees.Add(f)));
            menuEntity.Souce.ForEach(x => x.Employees.Where(d => !employees.Any(c => c == d)).ToList()
    .ForEach(f => employees.Add(f)));
            return employees;
        }

        protected List<string> GetEmployeesGuid(List<string> EmployeesIds)
        {
            var employees = new List<string>();
            var repo = new CodeMashRepository<EmployeeEntity>(Client);
            foreach (var employee in EmployeesIds)
            {
                var person = repo.FindOneById(
                        employee,
                        new DatabaseFindOneOptions()
                    );
                if (person.User != null)
                {
                    employees.Add(person.User);
                }
                
            }

            return employees;
        }


    }
}