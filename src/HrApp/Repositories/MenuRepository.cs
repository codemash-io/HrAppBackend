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

            await service.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Pull($"main_dish_options.$[].employees", employeeEntity.Id)
            );
            
            await service.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Pull($"soups.$[].employees", employeeEntity.Id)
            );
            
            await service.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Pull($"drinks.$[].employees", employeeEntity.Id)
            );
            
            await service.UpdateOneAsync(x => x.Id == menu.Id,
                Builders<MenuEntity>.Update.Pull($"souces.$[].employees", employeeEntity.Id)
            );

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

        /// <summary>
        /// Gets all employees who can order the food and haven't done it yet
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<List<Guid>> GetEmployeesWhoCanOrderFood(Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);
            var menuResponse = await repo.FindOneAsync(x => x.Id == menu.Id);
            
            // all employees who are allowed to order the food (not in holidays,...)
            var allEmployees = menuResponse.Employees;
            
            // Check who already did food reservation
            var employees = new List<string>();
            
            if (menuResponse.MainFood != null && menuResponse.MainFood.Any())
                employees.AddRange(menuResponse.MainFood.SelectMany(x => x.Employees));
            if (menuResponse.Soup != null && menuResponse.Soup.Any())
                employees.AddRange(menuResponse.Soup.SelectMany(x => x.Employees));
            if (menuResponse.Souces != null && menuResponse.Souces.Any())
                employees.AddRange(menuResponse.Souces.SelectMany(x => x.Employees));
            if (menuResponse.Drinks != null && menuResponse.Drinks.Any())
                employees.AddRange(menuResponse.Drinks.SelectMany(x => x.Employees));


            // Collect only employees who don't have reservation yet
            var employeesWhoCanDoReservation = allEmployees.Except(employees.Distinct());
            
            var employeesRepo = new CodeMashRepository<EmployeeEntity>(Client);
            var filter = Builders<EmployeeEntity>.Filter.In(x => x.Id, employeesWhoCanDoReservation); 
            var projection = Builders<EmployeeEntity>.Projection.Include(x => x.UserId); 
            var response = await employeesRepo.FindAsync<EmployeeEntity>(filter, projection);
            
            return response.Items
                .Where(x => x.UserId != Guid.Empty)
                .Select(x => x.UserId)
                .ToList();

        }

        public async Task<List<Guid>> GetEmployeesWhoOrderedFood(Menu menu)
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);
            var menuResponse = await repo.FindOneAsync(x => x.Id == menu.Id);
            
            
             // Check who already did food reservation
            var employees = new List<string>();
            
            if (menuResponse.MainFood != null && menuResponse.MainFood.Any())
                employees.AddRange(menuResponse.MainFood.SelectMany(x => x.Employees));
            if (menuResponse.Soup != null && menuResponse.Soup.Any())
                employees.AddRange(menuResponse.Soup.SelectMany(x => x.Employees));
            if (menuResponse.Souces != null && menuResponse.Souces.Any())
                employees.AddRange(menuResponse.Souces.SelectMany(x => x.Employees));
            if (menuResponse.Drinks != null && menuResponse.Drinks.Any())
                employees.AddRange(menuResponse.Drinks.SelectMany(x => x.Employees));
            
            
            var employeesRepo = new CodeMashRepository<EmployeeEntity>(Client);
            var filter = Builders<EmployeeEntity>.Filter.In(x => x.Id, employees.Distinct()); 
            var projection = Builders<EmployeeEntity>.Projection.Include(x => x.UserId); 
            var response = await employeesRepo.FindAsync<EmployeeEntity>(filter, projection);
            
            return response.Items
                .Where(x => x.UserId != Guid.Empty)
                .Select(x => x.UserId)
                .ToList();
        }

        public async Task<Menu> GetClosestMenu()
        {
            var repo = new CodeMashRepository<MenuEntity>(Client);
            var filter = Builders<MenuEntity>.Filter.Eq("status", "InProcess");
            var sort = Builders<MenuEntity>.Sort.Ascending(x => x.PlannedDate);
            var response = await repo.FindAsync(filter, sort);

            if (response == null || !response.Items.Any())
            {
                throw new BusinessException("Cannot find menu");
            }

            var closestMenuByDate = response.Items.FirstOrDefault();
            
            if (closestMenuByDate == null)
            {
                throw new BusinessException("Cannot find menu in database");
            }
            
            return new Menu(
                closestMenuByDate.PlannedDate, 
                new Division { Id = closestMenuByDate.DivisionId }, 
                closestMenuByDate.Employees.Select(x => new EmployeeEntity { Id = x}).ToList()
            ) { Id = closestMenuByDate.Id };
        }
    }
}