namespace Stationary_Management.Migrations
{
    using SCHM.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SCHM.Repo.SCHMDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SCHM.Repo.SCHMDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var seedRoles = new List<UserRole>
            {
                new UserRole {RoleName = "Administrator", Status = 2},
                new UserRole {RoleName = "Admin", Status = 1},
                new UserRole {RoleName = "Manager", Status = 1},
            };
            seedRoles.ForEach(s => context.UserRoles.AddOrUpdate(r => r.RoleName, s));
            context.SaveChanges();
            var seedRolePermissionList = new List<UserRolePermission>
            {
                new UserRolePermission{RoleId = context.UserRoles.FirstOrDefault(x => x.RoleName == "Administrator").Id, Permission = "Global_SupAdmin"},

            };
            seedRolePermissionList.ForEach(s => context.UserRolePermissions.AddOrUpdate(u => u.Permission, s));
            context.SaveChanges();

            //var company = new List<Company>
            //{
            //    new Company{ Name="HuntsMan", Address="England", Phone="+009", Email="HM@yahoo.com" ,Status=1}
            //};
            //company.ForEach(s => context.Companies.AddOrUpdate(u => u.Name, s));
            //context.SaveChanges();

            #region users seed
            var users = new List<User>
            {
                new User { Name="Administrator",UserName = "admin",Password = "827ccb0eea8a706c4c34a16891f84e7b",RoleId = context.UserRoles.FirstOrDefault(x => x.RoleName == "Administrator").Id,SupUser = true, Status=2,UserType="Admin",LastPassChangeDate = DateTime.Now,PasswordChangedCount=1,EmployeeType="Permanent"},//12345
                new User { Name="Development",UserName = "dev",Password = "827ccb0eea8a706c4c34a16891f84e7b",RoleId = context.UserRoles.FirstOrDefault(x => x.RoleName == "Administrator").Id,SupUser = true, Status=2,UserType="Admin",LastPassChangeDate = DateTime.Now,PasswordChangedCount=1,EmployeeType="Permanent"}, //12345
                new User { Name="Md. Rasel Parvez",UserName = "Parvez",Password = "827ccb0eea8a706c4c34a16891f84e7b",RoleId = context.UserRoles.FirstOrDefault(x => x.RoleName == "Administrator").Id,SupUser = true, Status=2,UserType="Admin",LastPassChangeDate = DateTime.Now,PasswordChangedCount=1,EmployeeType="Permanent"}, //12345

            };
            users.ForEach(s => context.Users.AddOrUpdate(u => u.UserName, s));
            context.SaveChanges();
            #endregion


        }
    }
}
