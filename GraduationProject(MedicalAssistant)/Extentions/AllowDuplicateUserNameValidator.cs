using Microsoft.AspNetCore.Identity;

namespace GraduationProject_MedicalAssistant_.Extentions
{
    public class AllowDuplicateUserNameValidator<TUser>
     : IUserValidator<TUser> where TUser : class
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            // Skip username uniqueness check — allow duplicates
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
