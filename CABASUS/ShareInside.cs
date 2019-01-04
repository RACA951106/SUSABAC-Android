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
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using CABASUS.Modelos;
using Firebase.Iid;
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
        public async Task<string> ConsultarToken()
        {
            try
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
            catch (System.Exception)
            {
                return "";
            }
        }
        public async Task<string> LogearUsuario(login log)
        {
            var contenido = "";
            try
            {
                if (HayConexion())
                {
                    string url = "http://192.168.0.10:5001/api/account/Login";
                    var json = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
                    HttpClient cliente = new HttpClient();
                    cliente.Timeout = TimeSpan.FromSeconds(20);
                    var respuesta = await cliente.PostAsync(url, json);
                    respuesta.EnsureSuccessStatusCode();
                    if (respuesta.IsSuccessStatusCode)
                    {
                        contenido = await respuesta.Content.ReadAsStringAsync();
                        var cont = JsonConvert.DeserializeObject<Token>(contenido);
                        var datosusuario = JsonConvert.DeserializeObject<usuarios>(contenido);
                        datosusuario.contrasena = log.contrasena;
                        Guardar_DatosUsuario(datosusuario);
                        GuardarToken(cont);
                        Guardar_Email_Contrasena(log.usuario, log.contrasena);
                        return "Logeado";
                    }
                    else
                    {
                        return await respuesta.Content.ReadAsStringAsync();
                    }
                }
                else
                    return "No hay conexion";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
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

        public void GuardarCaballos(List<consultacompartidos> ListaCaballos)
        {
            var serializador = new XmlSerializer(typeof(List<consultacompartidos>));
            var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ListaCaballosPropiosYCompartidos.xml"));
            serializador.Serialize(Escritura, ListaCaballos);
            Escritura.Close();
        }
        
        public async Task<string> DownloadImageAsync(string imageUrl, string id_caballo)
        {
            try
            {
                if (HayConexion())
                {
                    const int _downloadImageTimeoutInSeconds = 15;
                    HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };
                    using (var httpResponse = await _httpClient.GetAsync(imageUrl))
                    {
                        if (httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            var img = await httpResponse.Content.ReadAsByteArrayAsync();

                            var bitmap = Bitmap.CreateScaledBitmap(BitmapFactory.DecodeByteArray(img, 0, img.Length), 100, 100, false);

                            byte[] newImg;
                            using (var stream = new MemoryStream())
                            {
                                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                                newImg = stream.ToArray();
                            }

                            Java.IO.File _dir = new Java.IO.File(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)), "TemporalCaballos");
                            if (!_dir.Exists())
                                _dir.Mkdirs();
                            Java.IO.File _file = new Java.IO.File(_dir, id_caballo + ".jpg");
                            Android.Net.Uri Uri_Save = Android.Net.Uri.FromFile(_file);
                            File.WriteAllBytes(Uri_Save.Path, newImg);
                            return Uri_Save.Path;
                        }
                        else
                            return "No hay conexion";
                    }
                }
                else
                    return "No hay conexion";
            }
            catch (System.Exception)
            {
                return "No hay conexion";
            }
        }
        
        public void Guardar_DatosUsuario(usuarios user)
        {
            var guardarususario = new usuarios();
            guardarususario.id_usuario = user.id_usuario;
            guardarususario.email = user.email;
            guardarususario.contrasena = user.contrasena;
            guardarususario.fecha_nacimiento = user.fecha_nacimiento;
            guardarususario.nombre = user.nombre;
            guardarususario.foto = user.foto;
            var serializador = new XmlSerializer(typeof(usuarios));
            var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatosUsuario.xml"));
            serializador.Serialize(Escritura, guardarususario);
            Escritura.Close();
        }
        public usuarios Consultar_DatosUsuario()
        {
            var serializador = new XmlSerializer(typeof(usuarios));
            var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatosUsuario.xml"));
            var datos = (usuarios)serializador.Deserialize(Lectura);
            Lectura.Close();
            return datos;
        }

        public async Task<string> GenerarTokenFirebase()
        {
            HttpClient Clientelogin = new HttpClient(new System.Net.Http.HttpClientHandler());
            await Task.Delay(1000);
            string token = FirebaseInstanceId.Instance.Token;
            await Task.Delay(3000);
            Log.Debug("tag", token);
            return token;
        }

        public List<consultacompartidos> ConsultarCaballos()
        {
            var serializador = new XmlSerializer(typeof(List<consultacompartidos>));
            var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ListaCaballosPropiosYCompartidos.xml"));
            var datos = (List<consultacompartidos>)serializador.Deserialize(Lectura);
            Lectura.Close();
            return datos;
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
      
    }
    
}