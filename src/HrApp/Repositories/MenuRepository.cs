using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Client;
using CodeMash.Repository;
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
            return response.Result.Id;
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
                await service.UpdateOneAsync(x => x.Id == menu.Id,
                    Builders<MenuEntity>.Update.AddToSet($"main_dish_options.items[{mainCourse.FoodNumber}].employees", employeeEntity.Id), null);
            }
            
            var soup = FindPreference(FoodType.Soup);

            if (soup != null)
            {
                await service.UpdateOneAsync(x => x.Id == menu.Id,
                    Builders<MenuEntity>.Update.AddToSet($"soups.items[{soup.FoodNumber}].employees", employeeEntity.Id), null);
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

            var response = await repo.FindOneAsync(x => x.Id == menu.Id);
            var menuEntity = response.Result;
            var employees = GetEmployeesList(menuEntity);

            return employees.Select(Guid.Parse).ToList();
        }

        async Task<List<Guid>> IMenuRepository.GetEmployeesWhoStillNotMadeAnOrder(Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);

            var response = await repo.FindOneAsync(x => x.Id == menu.Id);
            var menuEntity = response.Result;
            var employees = GetEmployeesList(menuEntity);

            var employeeswoOrders = menuEntity.Employees.Except(employees).ToList();

            return employeeswoOrders.Select(Guid.Parse).ToList();
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

    }
}