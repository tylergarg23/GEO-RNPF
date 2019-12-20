using Modulos_Core_MVC.Models;
using System.Threading.Tasks;
using SERFOR.Component.Tools.Cryptography;
using SERFOR.Component.DTEntities.Seguridad;
using SERFOR.Component.SeguridadCore.BusinessLogic.Facade;

namespace Modulos_Core_MVC.Security
{
    public class SerforUserStore<T> : UserStoreAdapter<ApplicationUser>
    {
        public override Task<ApplicationUser> FindByNameAsync(string nombre)
        {
            UsuarioLoginDTe usuario = new UsuarioStore(nombre).Usuario;
            if (usuario != null)
            {
                return Task.FromResult(new ApplicationUser { Usuario = usuario, UserName = nombre });
            }
            return Task.FromResult(default(ApplicationUser));
        }

        public override Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(HashCrypter.Sha1Encrypter(user.Usuario.Password));
        }

        public override Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }
    }
}