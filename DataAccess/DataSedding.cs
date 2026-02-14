using System;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DataAccess
{
    public class DataSedding
    {

        public static void SpecilzationSeed(ApplicationDbContext dbContext, ILogger logger)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream("DataAccess.DataSeed.specification.json");
                using var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                var list = JsonSerializer.Deserialize<List<Specialization>>(data);

               
                if (list == null || !list.Any())
                {
                    logger.LogWarning("No data found in specification.json");
                    return;
                }

                if (!dbContext.Specializations.Any())
                {
                    dbContext.Specializations.AddRange(list);
                    dbContext.SaveChanges();
                    logger.LogInformation("Specializations seeding completed successfully.");
                    Console.WriteLine("Specializations seeding completed successfully."); // للتأكيد

                }
                else
                {
                    logger.LogInformation("Specializations already exist, skipping seeding.");
                    Console.WriteLine("Specializations already exist, skipping seeding."); // للتأكيد

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during Specializations seeding");
                Console.WriteLine("Error during Specializations seeding: " + ex.Message);

                throw;
            }
        }

    }
} 

