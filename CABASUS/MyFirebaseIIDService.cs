using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using System.IO;
using Android.OS;
using System.Net.Http;

namespace RecibirNotificcion.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override async void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
            if (File.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatosUsuario.xml")))
            {
                string url = "http://192.168.0.10:5001/api/actualizarTokenFB/"+refreshedToken+"/"+ Build.Serial;
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                if (new CABASUS.ShareInside().HayConexion())
                {
                    cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await new CABASUS.ShareInside().ConsultarToken());
                    var respuesta = await cliente.GetAsync(url);
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    if (respuesta.IsSuccessStatusCode)
                    {
                         Console.WriteLine("Datos Guardados");
                    }

                }
            }
        }
        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}