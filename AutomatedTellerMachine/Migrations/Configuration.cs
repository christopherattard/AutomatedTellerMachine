namespace AutomatedTellerMachine.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MVCServices;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<AutomatedTellerMachine.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AutomatedTellerMachine.Models.ApplicationDbContext";
        }

        protected override void Seed(AutomatedTellerMachine.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(t => t.UserName == "admin@mvcatm.com"))
            {
                var user = new ApplicationUser { UserName = "admin@mvcatm.com", Email = "admin@mvcatm.com" };
                userManager.Create(user, "Pass!23");

                var service = new CheckingAccountServices(context);
                service.CreateCheckingAccount("admin", "user", user.Id, 1000);

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
            }

            //context.Transactions.Add(new Transaction { Amount = -1.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 4.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 5.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 61.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -1.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 21.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -21.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -41.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 56.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 767.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 23.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 1.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 431.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -3.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -143.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -11.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -5.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -2.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = -31.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 221.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 41.50m, CheckingAccountId = 5 });
            //context.Transactions.Add(new Transaction { Amount = 77.50m, CheckingAccountId = 5 });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
