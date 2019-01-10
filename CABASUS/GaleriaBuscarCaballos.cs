using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CABASUS.Modelos;
using Galeria;
using System.IO;
using Newtonsoft.Json;
using Android.Text;
using Android.Support.V4.Widget;
using Android.Graphics;

namespace CABASUS
{
    [Activity(Label = "GaleriaBuscarCaballos")]
    public class GaleriaBuscarCaballos : Activity
    {
        GridView grid;
        EditText txtbusqueda;
        List<caballos> listacaballos;
        protected async override  void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GridLayout);
            grid = FindViewById<GridView>(Resource.Id.gridview);
            txtbusqueda = FindViewById<EditText>(Resource.Id.editText);
            var refresh = FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            var metrics = Resources.DisplayMetrics;
            listacaballos = await FeedGaleria();
            grid.Adapter = new AdaptadorGrid(listacaballos, this, metrics);
            txtbusqueda.TextChanged += async (object sender, TextChangedEventArgs e) =>
            {
                listacaballos = await FeedBusqueda(txtbusqueda.Text);
                grid.Adapter = new AdaptadorGrid(listacaballos, this, metrics);
            };
            #region Refrescar con scroll

            refresh.SetColorSchemeColors(Color.Rgb(230,30, 30));
            refresh.SetProgressBackgroundColorSchemeColor(Color.Rgb(255, 255, 255));
            refresh.Refresh += async delegate {

                #region llenar lista de caballos segun la conexion 
                if (new ShareInside().HayConexion())
                {
                    listacaballos = await FeedGaleria();
                    grid.Adapter = new AdaptadorGrid(listacaballos, this, metrics);
                }
                #endregion;
                refresh.Refreshing = false;
            };


            #endregion


        }
        public async Task<List<caballos>> FeedGaleria()
        {
            #region Generar lista de imagenes
            List<Modelos.caballos> Lista = new List<Modelos.caballos>();
            try
            {
                string c;
                string url = "http://192.168.1.74:5001/api/caballo/caballosaleatorios";
                var tok = await new ShareInside().ConsultarToken();
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tok);
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var respuesta = await cliente.GetAsync(url);
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    Lista= JsonConvert.DeserializeObject<List<Modelos.caballos>>(contenido);
                }
                else
                    c = await respuesta.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                var msj = ex.Message;
            }
            return Lista;
            #endregion
        }

        public async Task<List<caballos>> FeedBusqueda(string texto)
        {
            #region Generar lista de imagenes
            List<Modelos.caballos> Lista = new List<Modelos.caballos>();
            try
            {
                string c;
                string url = "http://192.168.1.74:5001/api/caballo/buscarcaballos?refe=" + texto;
                var tok = await new ShareInside().ConsultarToken();
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tok);
                cliente.Timeout = TimeSpan.FromSeconds(20);
                var respuesta = await cliente.GetAsync(url);
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    Lista = JsonConvert.DeserializeObject<List<Modelos.caballos>>(contenido);
                }
                else
                    c = await respuesta.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                var msj = ex.Message;
            }
            return Lista;
            #endregion
        }
    }
}