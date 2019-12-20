using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Modulos_Core_MVC.Models;
using Modulos_Core_MVC.Security;
using SERFOR.Component.DTEntities.Seguridad;
using SERFOR.Component.Tools.Cryptography;

namespace Modulos_Core_MVC
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new SerforUserStore<ApplicationUser>());

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public ClaimsIdentity CreateUserIdentity(ApplicationUser user)
        {
            var identy = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Name, ClaimTypes.Role);
            UsuarioLoginDTe usuario = user.Usuario;
            identy.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.NombreUsuario));
            identy.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identy.AddClaim(new Claim(ClaimTypes.GivenName, usuario.NombreCompleto));
            identy.AddClaim(new Claim(ClaimTypes.Locality, usuario.Sede));
            identy.AddClaim(new Claim(ClaimTypes.Email, usuario.Email));
            identy.AddClaim(new Claim("PersonaId", usuario.Id.ToString()));
            identy.AddClaim(new Claim("SedeId", usuario.Sede_Id.ToString()));
            identy.AddClaim(new Claim("SedePrincipalId", usuario.SedePrincipal_Id.ToString()));
            identy.AddClaim(new Claim("EsSedePrincipal", usuario.EsSedePrincipal.ToString()));
            usuario.Roles.ForEach(r =>
            {
                identy.AddClaim(new Claim(ClaimTypes.Role, r.Codigo));
            });
            return identy;
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            var appuser = await Store.FindByNameAsync(userName);

            return (appuser != null && appuser.Usuario.Password.Equals(HashCrypter.Sha1Encrypter(password))) ? appuser : null;
        }
    }
}
