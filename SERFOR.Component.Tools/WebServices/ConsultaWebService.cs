using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SERFOR.Component.Tools.WebServices
{
    public class ConsultaWebService
    {
        public ConsultaWebService()
        {

        }
        public Empresa getDatosEmpresa(string ruc)
        {

            Empresa empresa = new Empresa();
            HttpWebRequest request = CreateWebRequest("http://wscr.sunat.gob.pe:90/cl-ti-iaconsulruc-ws/services/ConsultaRuc?wsdl");
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://service.consultaruc.registro.servicio2.sunat.gob.pe"">
                  <soapenv:Header/>
                  <soapenv:Body>
                    <ser:getDatosPrincipales soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                        <numruc xsi:type=""xsd: string"">" + ruc + @"</numruc>                     
		            </ser:getDatosPrincipales>
                  </soapenv:Body>
                </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();


                    XmlDocument document = new XmlDocument();
                    document.LoadXml(soapResult);
                    XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);

                    //manager.AddNamespace("S", "http://schemas.xmlsoap.org/soap/envelope/");
                    //manager.AddNamespace("ns2", "http://ws.webservice.serfor.gob.pe/");

                    XmlNodeList xnList = document.SelectNodes("//multiRef", manager);
                    int nodes = xnList.Count;

                    foreach (XmlNode xn in xnList)
                    {
                        if (xn["ddp_numruc"].InnerText == ruc)
                        {
                            empresa.RUC = xn["ddp_numruc"].InnerText;
                            empresa.RazonSocial = xn["ddp_nombre"].InnerText;
                            empresa.Departamento = xn["desc_dep"].InnerText;
                            empresa.Provincia = xn["desc_prov"].InnerText;
                            empresa.Distrito = xn["desc_dist"].InnerText;
                            empresa.TipoVia = xn["desc_tipvia"].InnerText;
                            empresa.NombreVia = xn["ddp_nomvia"].InnerText;
                            empresa.NumeroDireccion = xn["ddp_numer1"].InnerText;
                            empresa.TipoZona = xn["desc_tipzon"].InnerText;
                            empresa.NombreZona = xn["ddp_nomzon"].InnerText;
                            empresa.ReferenciaDireccion = xn["ddp_refer1"].InnerText;
                            empresa.Rubro = xn["desc_ciiu"].InnerText;
                        }
                    }
                }
            }
            return empresa;
        }
        public string getDireccion(string ruc)
        {
            string direccion = "";
            HttpWebRequest request = CreateWebRequest("http://wscr.sunat.gob.pe:90/cl-ti-iaconsulruc-ws/services/ConsultaRuc?wsdl");
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://service.consultaruc.registro.servicio2.sunat.gob.pe"">
                <soapenv:Header/>
                <soapenv:Body>
                    <ser:getDomicilioLegal soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                        <numruc xsi:type=""xsd:string"">" + ruc + @"</numruc>
                    </ser:getDomicilioLegal>
                </soapenv:Body>
                </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();


                    XmlDocument document = new XmlDocument();
                    document.LoadXml(soapResult);
                    XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);

                    //manager.AddNamespace("S", "http://schemas.xmlsoap.org/soap/envelope/");
                    //manager.AddNamespace("ns2", "http://ws.webservice.serfor.gob.pe/");

                    XmlNodeList xnList = document.SelectNodes("//getDomicilioLegalReturn", manager);
                    int nodes = xnList.Count;

                    foreach (XmlNode xn in xnList)
                    {
                        direccion = xn.InnerText;
                    }
                }
            }
            return direccion;
        }
        public static HttpWebRequest CreateWebRequest(string ruta)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ruta);

            webRequest.Headers.Add("SOAPAction", "");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            /*Habilitar Proxy de manera Local*/
            ServicePointManager.Expect100Continue = false;
            webRequest.Proxy = HttpWebRequest.GetSystemWebProxy();
            webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

            return webRequest;

        }
    }
}
