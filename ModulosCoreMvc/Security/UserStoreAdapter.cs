using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Modulos_Core_MVC.Security
{
    public abstract class UserStoreAdapter<TUser> : IUserStore<TUser>, IUserPasswordStore<TUser>, IUserLockoutStore<TUser, string>
       where TUser : IdentityUser
    {
        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            IdentityUser user = new IdentityUser
            {
                Id = userId
            };
            return Task.FromResult(user as TUser);
        }

        public abstract Task<TUser> FindByNameAsync(string userName);

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public abstract Task<string> GetPasswordHashAsync(TUser user);

        public Task<bool> HasPasswordAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public abstract Task SetPasswordHashAsync(TUser user, string passwordHash);

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
    }
}