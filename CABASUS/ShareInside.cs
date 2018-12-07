using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using CABASUS.Modelos;
using Java.Lang;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using static Android.App.DatePickerDialog;

namespace CABASUS
{
   public class ShareInside
    {
        public bool HayConexion()
        {
            string CheckUrl = "https://www.google.com/";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;
            }
            catch (WebException ex)
            {
                // Console.WriteLine (".....no connection..." + ex.ToString ());
                return false;
            }
        }

        public async Task<string> SubirImagen(string Contenedor, string Nombre, Android.Net.Uri RutaArchivo)
        {
            if (HayConexion())
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=susabac;AccountKey=p6EvYU5CRlr7U3nXEp0A+Q/M1ZRtReQjomO8EwaBJ00LxKoo/7MG/m7aX7pbdJGGcJ0HcYGzn6LM7lFYbMeR+g==;EndpointSuffix=core.windows.net");
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Contenedor);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Nombre + ".jpg");
                cloudBlockBlob.Properties.ContentType = "image/jpg";

                try
                {
                    await cloudBlockBlob.UploadFromFileAsync(RutaArchivo.Path);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                catch (System.Exception ex)
                {
                    return "No hay conexion";
                }
            }
            else
            {
                return "No hay conexion";
            }
        }

        public void FormatoFecha(EditText txtDOB)
        {
            var noLetras = txtDOB.Text.Replace("/", "");
            noLetras = noLetras.Replace(" ", "");
            try
            {
                int.Parse(noLetras);

                if (txtDOB.Text.Length == 2)
                {
                    txtDOB.Text += " / ";
                }
                if (txtDOB.Text.Length == 7)
                {
                    txtDOB.Text += " / ";
                }
                if (txtDOB.Text.Length >= 15)
                {
                    txtDOB.Text = txtDOB.Text.Substring(0, txtDOB.Text.Length - 1);
                }
                if (txtDOB.Text.Length == 4)
                {
                    txtDOB.Text = txtDOB.Text = txtDOB.Text.Substring(0, txtDOB.Text.Length - 4);
                }
                if (txtDOB.Text.Length == 9)
                {
                    txtDOB.Text = txtDOB.Text = txtDOB.Text.Substring(0, txtDOB.Text.Length - 4);
                }
                txtDOB.SetSelection(txtDOB.Text.Length);
            }
            catch
            {
                txtDOB.Hint = "DD/MM/YYYY";
            }
        }
        
        public void Guardar_Email_Contrasena(string email, string contrasena)
        {
            var guardartoken = new ConsultarEmail();
            guardartoken.email = email;
            guardartoken.contrasena = contrasena;
            var serializador = new XmlSerializer(typeof(ConsultarEmail));
            var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConsultarEmail.xml"));
            serializador.Serialize(Escritura, guardartoken);
            Escritura.Close();
        }
        private ConsultarEmail Consultar_Email_Contrasena()
        {
            var serializador = new XmlSerializer(typeof(ConsultarEmail));
            var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConsultarEmail.xml"));
            var datos = (ConsultarEmail)serializador.Deserialize(Lectura);
            Lectura.Close();
            return datos;
        }
        public void GuardarToken(Token tokens)
        {
            var guardartoken = new Token();
            guardartoken.token = tokens.token;
            guardartoken.expiration = tokens.expiration;
            var serializador = new XmlSerializer(typeof(Token));
            var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Token.xml"));
            serializador.Serialize(Escritura, guardartoken);
            Escritura.Close();
        }
        private string ConsultarExpiracion()
        {
            var serializador = new XmlSerializer(typeof(Token));
            var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Token.xml"));
            var datos = (Token)serializador.Deserialize(Lectura);
            Lectura.Close();
            return datos.expiration;
        }
        public async Task<string> ConsultarTokenAsync()
        {
            var expiracion = Convert.ToDateTime(ConsultarExpiracion());
            var fechaactual = DateTime.Now;
            if (fechaactual >= expiracion)
            {
                login log = new login()
                {
                    usuario = Consultar_Email_Contrasena().email,
                    contrasena = Consultar_Email_Contrasena().contrasena,
                    TokenFB = "dsfsdf",
                    SO = "Android",
                    id_dispositivo = Build.Serial
                };
                if (HayConexion())
                {
                    await LogearUsuario(log);
                }
            }
            var serializador = new XmlSerializer(typeof(Token));
            var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Token.xml"));
            var datos = (Token)serializador.Deserialize(Lectura);
            Lectura.Close();
            return datos.token;
        }
        public async Task<string> LogearUsuario(login log)
        {
            string url = "http://192.168.1.73:5001/api/account/Login";
            var json = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
            HttpClient cliente = new HttpClient();
            var respuesta = await cliente.PostAsync(url, json);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var cont = JsonConvert.DeserializeObject<Token>(contenido);
                GuardarToken(cont);
                return "Logeado";
            }
            else
            {
                return await respuesta.Content.ReadAsStringAsync();
            }
        }

        public void CopyDocuments(string FileName, string AssetsFileName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(path, FileName);

            try
            {
                if (!File.Exists(dbPath))
                {
                    using (var br = new BinaryReader(Application.Context.Assets.Open(AssetsFileName)))
                    {
                        using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
    public class ObtenerDialogFecha
    {
        public class BaseActivity : AppCompatActivity
        {


            protected const int REQUEST_STORAGE_READ_ACCESS_PERMISSION = 101;
            protected const int REQUEST_STORAGE_WRITE_ACCESS_PERMISSION = 102;

            private Android.Support.V7.App.AlertDialog mAlertDialog;


            protected override void OnStop()
            {
                base.OnStop();
                if (mAlertDialog != null && mAlertDialog.IsShowing)
                {
                    mAlertDialog.Dismiss();
                }
            }

            protected void RequestPermission(string permission, string rationale, int requestCode)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
                {
                    ShowAlertDialog("permission_title_rationale",
                                rationale,
                                (sender, args) =>
                                {
                                    ActivityCompat.RequestPermissions(this, new System.String[] { permission }, requestCode);
                                },
                                "ok",
                                null,
                                "cancel");
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new System.String[] { permission }, requestCode);
                }
            }

            protected void ShowAlertDialog(string title, string message, EventHandler<DialogClickEventArgs> onPositiveButtionClicked, string positiveButtonText, EventHandler<DialogClickEventArgs> onNegativeButtionClicked, string negativeButtonText)
            {
                Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton(positiveButtonText, onPositiveButtionClicked);
                builder.SetNegativeButton(negativeButtonText, onNegativeButtionClicked);
                mAlertDialog = builder.Show();
            }
        }
        public class PickerDate : Java.Lang.Object, IOnDateSetListener
        {
            Button textoDate;
            TextView textoDateDiario;

            public PickerDate(Button textoDate)
            {
                this.textoDate = textoDate;
            }

            public PickerDate(TextView textoDateDiario)
            {
                this.textoDateDiario = textoDateDiario;
            }

            void IOnDateSetListener.OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
            {
                string m = (month + 1).ToString(), d = dayOfMonth.ToString();

                if (m.Length <= 1)
                    m = "0" + (month + 1).ToString();
                if (d.Length <= 1)
                    d = "0" + dayOfMonth.ToString();

                try { textoDate.Text = year.ToString() + "/" + m + "/" + d; } catch (System.Exception) { }
                try
                {
                    double Mes = (double.Parse(m) + 1);
                    if (Mes < 10)
                        m = "0" + Mes.ToString();
                    textoDateDiario.Text = d + "-" + m + "-" + year.ToString();
                }
                catch (System.Exception) { }
            }
        }
    }
    
}