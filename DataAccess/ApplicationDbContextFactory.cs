using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public class ApplicationDbContextFactory
     : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // تحديد مكان appsettings.json
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(@"..\GraduationProject(MedicalAssistant)")
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .Build();

            //var connectionString = configuration.GetConnectionString("DefaultConnection");

            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=GraduationProject_MedicalAssistant;Integrated Security=True;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
