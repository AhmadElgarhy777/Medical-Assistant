namespace GraduationProject_MedicalAssistant_.Extentions
{
   
    using Microsoft.AspNetCore.Identity;
    using Models;
    using Models.Enums;
    using Utility;

    public static class SuperAdminSeeder
    {
       
        public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider,IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // ── 1. Ensure SuperAdmin role exists ────────────────────────────────
            if (!await roleManager.RoleExistsAsync(SD.SuperAdminRole))
                await roleManager.CreateAsync(new IdentityRole(SD.SuperAdminRole));

            // ── 2. Check if Super Admin already seeded ──────────────────────────
            //  ⚠️ Change these values — store them in appsettings.json or secrets
             string superAdminEmail =configuration["SuperAdmin:Email"];
             string superAdminUserName = configuration["SuperAdmin:UserName"];
             string superAdminPassword = configuration["SuperAdmin:Password"];   // Change this!

            var existing = await userManager.FindByEmailAsync(superAdminEmail);
            if (existing != null) return;   // Already seeded, skip

            // ── 3. Create the Super Admin user ──────────────────────────────────
            var superAdminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = superAdminUserName,
                Email = superAdminEmail,
                EmailConfirmed = true,          // No need for email confirmation
                Role = SD.SuperAdminRole,
                Address = "Super Admin",
                City = "Super Admin",
                Governorate = Governorate.Cairo,
                Gender= GenderEnum.male
            };

            var result = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Super Admin seeding failed: {errors}");
            }

            await userManager.AddToRoleAsync(superAdminUser, SD.SuperAdminRole);
        }
    }

    // ── In Program.cs call it like this ─────────────────────────────────────────
    //
    //   var app = builder.Build();
    //   await SuperAdminSeeder.SeedSuperAdminAsync(app.Services);
    //   app.Run();
    //
    // ────────────────────────────────────────────────────────────────────────────

}
