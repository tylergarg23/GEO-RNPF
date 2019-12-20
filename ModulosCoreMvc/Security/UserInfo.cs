using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace Modulos_Core_MVC.Security
{
    public class UserInfo
    {
        public static string GetFullName()
        {
            return ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.GivenName).Select(e => e.Value).FirstOrDefault();
        }

        public static string GetUserName()
        {
            return ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(e => e.Value).FirstOrDefault();
        }

        public static string GetEmail()
        {
            return ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.Email).Select(e => e.Value).FirstOrDefault();
        }

        public static string GetLocality()
        {
            return ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.Locality).Select(e => e.Value).FirstOrDefault();
        }

        public static int? GetId()
        {
            string value = ClaimsPrincipal.Current.Claims.Where(c => c.Type == "PersonaId").Select(e => e.Value).FirstOrDefault();
            if (value != null && !value.Equals(""))
            {
                return int.Parse(value);
            }
            return null;
        }

        public static short? GetSedeId()
        {
            string value = ClaimsPrincipal.Current.Claims.Where(c => c.Type == "SedeId").Select(e => e.Value).FirstOrDefault();
            if (value != null && !value.Equals(""))
            {
                return short.Parse(value);
            }
            return null;
        }
        public static short? GetSedePrincipalId()
        {
            string value = ClaimsPrincipal.Current.Claims.Where(c => c.Type == "SedePrincipalId").Select(e => e.Value).FirstOrDefault();
            if (value != null && !value.Equals(""))
            {
                return short.Parse(value);
            }
            return null;
        }

        public static bool? GetEsSedePrincipal()
        {
            string value = ClaimsPrincipal.Current.Claims.Where(c => c.Type == "EsSedePrincipal").Select(e => e.Value).FirstOrDefault();
            if (value != null && !value.Equals(""))
            {
                return bool.Parse(value);
            }
            return null;
        }

        public static bool HasOneRole(string strRoles)
        {
            bool has = false;
            string[] roles = strRoles.Split(',');
            foreach (string role in roles)
            {
                if (ClaimsPrincipal.Current.IsInRole(role.Trim()))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
    }
}