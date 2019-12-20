using SERFOR.Component.DTEntities.General;
using SERFOR.Component.GeneralCore.DataAccess;
using System;
using System.Linq;
using System.Text;

namespace SERFOR.Component.GeneralCore.BusinessLogic.AbstractFactory.CodigoDerecho
{
    public abstract class DerechoProvider
    {

        #region Constructores de la clase

        public DerechoProvider() { }

        public DerechoProvider(short sedeId, int ubigeoId)
        {
            GeneracionExitosa = true;
           
            try
            {
                using (var context = new GeneralSchema())
                {
                    var ubigeo = context.Ubigeo.Find(ubigeoId);
                    if (ubigeoId > 0 && ubigeo != null)
                    {
                        CodigoDepartamento = ubigeo.CodigoDepartamento;
                        NombreDepartamento = ubigeo.NombreDepartamento;
                    }

                    var sede = context.SedeAutoridadForestal.Find(sedeId);
                    if (sedeId > 0 && sede != null)
                    {
                        ATFFSId = sede.AutoridadForestal.Id;
                        CodigoSedeUA = sede.AutoridadForestal.Codigo;
                        NombreSedeUA = sede.AutoridadForestal.Nombre;
                        if (sede.AutoridadForestal.EsGobiernoRegional && !string.IsNullOrEmpty(sede.CodigoUA.Trim()))
                        {
                            CodigoSedeUA += "-" + sede.CodigoUA;
                            NombreSedeUA += "-" + sede.Nombre;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                GeneracionExitosa = false;
            }
        }

        #endregion

        /// <summary>
        /// Filtro año para la búsqueda del correlativo
        /// </summary>
        public short Annio { protected get; set; }

        /// <summary>
        /// Filtro ID de la Sede de la autoridad forestal para la búsqueda del correlativo
        /// </summary>
        public byte ATFFSId { protected get; set; }

        /// <summary>
        /// Codigo generado según metodología definida en el RDE N° 042-2016
        /// </summary>
        public string CodigoGenerado { get; protected set; }

        /// <summary>
        /// Cualquier cadena de información, incluyendo mensajes de error
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Cualquier cadena de información, incluyendo mensajes de error
        /// </summary>
        public bool GeneracionExitosa { get; set; }


        public string CodigoDepartamento { get; protected set; }

        public string NombreDepartamento { get; protected set; }

        public string CodigoSedeUA { get; protected set; }

        public string NombreSedeUA { get; protected set; }

        public string CodigoDerecho { get; protected set; }

        public string NombreDerecho { get; protected set; }

        public string CodigoRecursoActividad { get; protected set; }

        public string NombreRecursoActividad { get; protected set; }

        public string Correlativo { get; set; }


        public virtual string Componer(bool actualizarSemilla)
        {
            GeneracionExitosa = true;
            var resultado = string.Empty;
            resultado += validaCodigo(CodigoDepartamento);
            if(CodigoSedeUA.Trim()!="")
                resultado += "-" + validaCodigo(CodigoSedeUA);
            resultado += "/" + validaCodigo(CodigoDerecho);
            resultado += "-" + validaCodigo(CodigoRecursoActividad);
            resultado += "-" + validaCodigo(Annio.ToString());

            using (var context = new GeneralSchema())
            {
                var semilla = context.CodigoDerechoSemilla.Where(s => s.ATFFS_Id == ATFFSId && s.Annio == Annio).First();
                if (semilla == null)
                {
                    Correlativo = "001";
                    semilla = new CodigoDerechoSemilla
                    {
                        Annio = Annio,
                        ATFFS_Id = ATFFSId,
                        Correlativo = 0,
                        UsuarioRegistro = "system",
                        FechaRegistro = DateTime.Now
                    };

                    context.CodigoDerechoSemilla.Add(semilla);
                    context.SaveChanges();
                }
                else
                {
                    Correlativo = string.Format("{0:00#}", semilla.Correlativo + 1);
                    resultado += "-" + Correlativo;
                }

                if(actualizarSemilla)
                {
                    semilla.Correlativo += 1;
                    context.SaveChanges();
                }
            }

            CodigoGenerado = resultado;

            return resultado;
        }

        public virtual bool Descomponer(string codigo)
        {
            throw new NotImplementedException();
        }

        private string validaCodigo(string codigo)
        {
            string cadena = string.Empty;
            if (!string.IsNullOrEmpty(codigo))
                cadena = codigo;
            else
                GeneracionExitosa = false;

            return cadena;
        }

        public virtual bool ActualizaTablaSemilla(string codigo)
        {
            throw new NotImplementedException();
        }

    }
}