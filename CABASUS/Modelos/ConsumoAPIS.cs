using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CABASUS.Modelos
{
    public class ConsumoAPIS
    {
        public async Task<string> RegistrarCaballos(Modelos.caballos modeloCaballos)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.10:5001/api/caballo/registrar";
                string jsonContent = "application/json";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var json = JsonConvert.SerializeObject(modeloCaballos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new ShareInside().ConsultarToken());
                var respuesta = await cliente.PostAsync(server, new StringContent(json, Encoding.UTF8, jsonContent));
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.IsSuccessStatusCode)
                    return await respuesta.Content.ReadAsStringAsync();
                else
                    return "No hay conexion";
            }
            else
                return "No hay conexion";
        }

        public async Task<string> ActualizarFotoCaballo(Modelos.caballos modeloCaballos)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.10:5001/api/caballo/actualizarFoto";
                string jsonContent = "application/json";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var json = JsonConvert.SerializeObject(modeloCaballos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new ShareInside().ConsultarToken());
                var respuesta = await cliente.PostAsync(server, new StringContent(json, Encoding.UTF8, jsonContent));
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.IsSuccessStatusCode)
                    return await respuesta.Content.ReadAsStringAsync();
                else
                    return "No hay conexion";
            }
            else
                return "No hay conexion";
        }

        public async Task<string> RecuperarContrasena(string email, int idioma)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.10:5001/api/Account/recuperarPass?email=" + email+"&idioma="+idioma;
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var consulta = await cliente.GetAsync(server);
                consulta.EnsureSuccessStatusCode();
                if (consulta.IsSuccessStatusCode)
                    return await consulta.Content.ReadAsStringAsync();
                else
                    return "No hay conexion";
            }
            else
                return "No hay conexion";
        }

        public async Task<string> ConsultarCompartidos()
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.10:5001/api/Compartir/consultarcompartidos";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new ShareInside().ConsultarToken());
                var consulta = await cliente.GetAsync(server);
                consulta.EnsureSuccessStatusCode();
                if (consulta.IsSuccessStatusCode)
                {
                    var ConsultaJson = await consulta.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<consultacompartidos>>(ConsultaJson);
                    if (data.Count > 0)
                    {
                        new ShareInside().GuardarCaballos(data);
                        return "true";
                    }
                    else
                        return "false";
                }
                else
                    return "No hay conexion";
            }
            else
                return "No hay conexion";
        }

        public async Task<caballos> ConsultarCaballo_Id(string Id_Caballo)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.10:5001/api/Caballo/consultaridcaballo/"+Id_Caballo;
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new ShareInside().ConsultarToken());
                var consulta = await cliente.GetAsync(server);
                consulta.EnsureSuccessStatusCode();
                if (consulta.IsSuccessStatusCode)
                {
                    var datos = await consulta.Content.ReadAsStringAsync();
                    var modelo = JsonConvert.DeserializeObject<List<caballos>>(datos);
                    return modelo[0];
                }
                else
                    return new caballos() { id_caballo = "No hay conexion" };
            }
            else
                return new caballos() { id_caballo = "No hay conexion" };
        }

        public async Task<string> ActualizarCaballo(Modelos.caballos modeloCaballos)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.73:5001/api/caballo/actualizar";
                string jsonContent = "application/json";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var json = JsonConvert.SerializeObject(modeloCaballos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await new ShareInside().ConsultarToken());
                var respuesta = await cliente.PutAsync(server, new StringContent(json, Encoding.UTF8, jsonContent));
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.IsSuccessStatusCode)
                    return await respuesta.Content.ReadAsStringAsync();
                else
                    return "No hay conexion";
            }
            else
                return "No hay conexion" ;
        }
    }
}