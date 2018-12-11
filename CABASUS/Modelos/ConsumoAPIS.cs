using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace CABASUS.Modelos
{
    public class ConsumoAPIS
    {
        public async System.Threading.Tasks.Task<string> RegistrarCaballos(Modelos.caballos modeloCaballos)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.1.76:523/api/caballo/registrar";
                string jsonContent = "application/json";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var json = JsonConvert.SerializeObject(modeloCaballos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFyZUBnbWFpbC5jb20iLCJpZCI6ImYyZGUwYmE0MGIwYjQzZGI4NTY2YzFhZDBhOGRkMyIsImV4cCI6MTU0NDE5Njg2MCwiaXNzIjoiZG9taW5pby5jb20iLCJhdWQiOiJkb21pbmlvLmNvbSJ9.x6j2ZAjA3j1oG7jaJuvBHBbl8NnU_4lxukocBgifSUE");
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

        public async System.Threading.Tasks.Task<string> ActualizarFotoCaballo(Modelos.caballos modeloCaballos)
        {
            if (new ShareInside().HayConexion())
            {
                string server = " http://192.168.1.76:523/api/caballo/actualizarFoto";
                string jsonContent = "application/json";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var json = JsonConvert.SerializeObject(modeloCaballos);
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFyZUBnbWFpbC5jb20iLCJpZCI6ImYyZGUwYmE0MGIwYjQzZGI4NTY2YzFhZDBhOGRkMyIsImV4cCI6MTU0NDE5Njg2MCwiaXNzIjoiZG9taW5pby5jb20iLCJhdWQiOiJkb21pbmlvLmNvbSJ9.x6j2ZAjA3j1oG7jaJuvBHBbl8NnU_4lxukocBgifSUE");
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

        public async System.Threading.Tasks.Task<string> RecuperarContrasena(string email, int idioma)
        {
            if (new ShareInside().HayConexion())
            {
                string server = "http://192.168.0.22:5001/api/Account/recuperarPass?email=" + email+"&idioma="+idioma;
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
    }
}