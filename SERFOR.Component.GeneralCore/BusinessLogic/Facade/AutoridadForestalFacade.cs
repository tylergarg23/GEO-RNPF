using SERFOR.Component.DTEntities.General;
using SERFOR.Component.GeneralCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERFOR.Component.GeneralCore.BusinessLogic.Facade
{
    public static class AutoridadForestalFacade
    {
        public static IEnumerable<AutoridadForestalItemListDTe> GetAll()
        {
            IEnumerable<AutoridadForestalItemListDTe> attfss;

            var dbContext = new GeneralSchema();

            attfss = dbContext.AutoridadForestal.Select(af => new AutoridadForestalItemListDTe()
            {
                Id = af.Id,
                EsGobiernoRegional = af.EsGobiernoRegional,
                Nombre = af.Nombre,
                Codigo = af.Codigo,
                RutaLogo = (!string.IsNullOrEmpty(af.UriStorageLogo)) ? af.UriStorageLogo : string.Empty,
                NombreContacto = af.NombreContacto,
                Direccion = af.DireccionContacto,
                Email = af.EmailContacto,
                Telefono = af.TelefonoContacto
            });

            dbContext.AutoridadForestal.AsNoTracking();

            return attfss;
        }

        public static bool SetLogoUriFile(int autoridadId, string uri)
        {
            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.AutoridadForestal.Find(autoridadId);

                    if (efObject != null)
                    {
                        efObject.UriStorageLogo = uri;
                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;

        }

        public static bool UpdateDatosContacto(int autoridadId, string nombre, string direccion, string telefono, string email)
        {
            bool exito = true;

            using (var dbContext = new GeneralSchema())
            {
                try
                {
                    var efObject = dbContext.AutoridadForestal.Find(autoridadId);

                    if (efObject != null)
                    {
                        efObject.NombreContacto = nombre;
                        efObject.DireccionContacto = direccion;
                        efObject.TelefonoContacto = telefono;
                        efObject.EmailContacto = email;

                        dbContext.SaveChanges();
                    }
                    else
                    { exito = false; }
                }
                catch
                {
                    exito = false;
                }
            }
            return exito;
        }
    }
}
