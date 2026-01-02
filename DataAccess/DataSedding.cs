using System;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DataAccess
{
    public class DataSedding
    {
        
        public static void SpecilzationSeed(ApplicationDbContext dbContext)
        {
            var data = File.ReadAllText("../DataAccess/DataSeed/specification.json");
            var List = JsonSerializer.Deserialize<List<Specialization>>(data);

            try
            {
                if (dbContext.Specializations?.Count() == 0)
                {
                    if (List?.Count() > 0)
                    {
                        foreach (var item in List)
                        {
                           item.SpecializationId=Guid.NewGuid().ToString();
                             dbContext.Set<Specialization>().Add(item);
                        }
                        dbContext.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Specilzation seeding",ex.InnerException);
                
            }
        }
    }
}
