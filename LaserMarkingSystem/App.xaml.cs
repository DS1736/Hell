using InIWorkspace.Data;
using InIWorkspace.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;

namespace InIWorkspace
{
    public partial class App : Application
    {
        private AppDbContext _dbContext;

        // Default constructor without parameters
        public App()
        {
            InitializeComponent();
        }

        // Set startup window to NewWorkspace
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceProvider = ConfigureServices(); // Get service provider for DI
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>(); // Get the AppDbContext instance

            _dbContext.Database.Migrate(); // Ensure database is migrated
            SeedAdminUser(); // Add default admin users if necessary            

            base.OnStartup(e);
        }

        // Seed initial admin users if necessary
        private void SeedAdminUser()
        {
            // Remove existing Admins
            _dbContext.Admins.RemoveRange(_dbContext.Admins);
            _dbContext.SaveChanges();

            // Add new Admin users
            var adminUsers = new List<Admin>
            {
                new Admin { Username = "janit@newnop.net", Password = "Welcome123#" },
                new Admin { Username = "jyoti@newnop.net", Password = "Welcome123#" },
                new Admin { Username = "naga@newnop.net", Password = "Welcome123#" },
                new Admin { Username = "admin@izicode.com", Password = "AdmiZi@0513" }
            };

            try
            {
                _dbContext.Admins.AddRange(adminUsers);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to create Admin User! App may not work. Contact support.");
                Console.WriteLine(ex.Message);
            }
        }

        // Configure services (dependency injection)
        private IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Register AppDbContext with SQLite configuration
            serviceCollection.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));

            // Register App and any other services
            serviceCollection.AddSingleton<App>();

            return serviceCollection.BuildServiceProvider(); // Build the service provider
        }
    }
}
