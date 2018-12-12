using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Com.Yalantis.Ucrop;
using System.IO;
using Uri = Android.Net.Uri;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using static CABASUS.ObtenerDialogFecha;
using CABASUS.Modelos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityRegistroUsuario : BaseActivity, IDialogInterfaceOnClickListener
    {
        public void OnClick(IDialogInterface dialog, int which)
        {
            throw new NotImplementedException();
        }
        Refractored.Controls.CircleImageView Foto;
        ProgressBar progress;
        #region Tomar Foto e Imagen Galeria

        int camrequestcode = 100;
        Uri cameraUri;
        bool IsGalery = false;
        private const string TAG = "SampleActivity";
        Stream fotogaleria;

        private const int REQUEST_SELECT_PICTURE = 0x01;
        private const string SAMPLE_CROPPED_IMAGE_NAME = "SampleCropImage";

        #endregion
        string actualizar="1";
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_RegistroUsuario);
            Window.SetStatusBarColor(Color.Black);
            Window.SetNavigationBarColor(Color.Black);

            var txtUserName = FindViewById<EditText>(Resource.Id.txtUserNameRegistro);
            var txtEmail = FindViewById<EditText>(Resource.Id.txtEmailRegistroUsuario);
            var txtContrasena = FindViewById<EditText>(Resource.Id.txtContrasenaRegistroUsuario);
            var txtEdad = FindViewById<EditText>(Resource.Id.txtEdadRegistroUsuario);
            var Terminos = FindViewById<TextView>(Resource.Id.txtTerminosRegistroUsuario);
            var btnaceptar = FindViewById<TextView>(Resource.Id.btnRegistroUsuario);
            Foto = FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.btnfoto);
            progress = FindViewById<ProgressBar>(Resource.Id.progressBar);
            progress.IndeterminateDrawable.SetColorFilter(Android.Graphics.Color.Rgb(203,30,30), Android.Graphics.PorterDuff.Mode.Multiply);
            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(203, 30, 30));
            gdCreate.SetCornerRadius(500);
            btnaceptar.SetBackgroundDrawable(gdCreate);

            Drawable drawableImage = GetDrawable(Resource.Drawable.horse_icon);
            Bitmap bitDrawableImage = ((BitmapDrawable)drawableImage).Bitmap; 

            Foto.SetImageBitmap(bitDrawableImage);

            #region Obtener edad
            txtEdad.TextChanged += delegate { new ShareInside().FormatoFecha(txtEdad); };
            txtEdad.Click += delegate { txtEdad.SetSelection(txtEdad.Text.Length); };
            #endregion

            #region Obtener Foto
            Foto.Click += delegate
            {
                Dialog alertar = new Dialog(this, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(false);
                alertar.SetContentView(Resource.Layout.layout_CustomAlert);
                alertar.Show();

                alertar.FindViewById<Button>(Resource.Id.btnCancel).Click += delegate
                {
                    alertar.Dismiss();
                };

                alertar.FindViewById<Button>(Resource.Id.btnGalery).Click += delegate
                {
                    Intent intent = new Intent();
                    intent.SetType("image/*");
                    intent.SetAction(Intent.ActionGetContent);
                    intent.AddCategory(Intent.CategoryOpenable);
                    StartActivityForResult(Intent.CreateChooser(intent, GetText(Resource.String.select_image)), REQUEST_SELECT_PICTURE);
                    alertar.Dismiss();
                };

                alertar.FindViewById<Button>(Resource.Id.btnCamara).Click += delegate
                {
                    openCamara();
                    alertar.Dismiss();
                };
            };
            #endregion

            btnaceptar.Click += async delegate 
            {
                var contenido = "";
                var url_foto = "";
                var fechavalidacion = true;
                try
                {
                    if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        Toast.MakeText(this, "Campos vacios verifica la información", ToastLength.Short).Show();
                    }
                    else if (!validar_email(txtEmail))
                    {
                        Toast.MakeText(this, "Verifica Email", ToastLength.Short).Show();
                    }
                    else 
                    {
                        if (txtEdad.Text.Length > 0)
                        {
                            try
                            {
                                Convert.ToDateTime(txtEdad.Text);
                                if (Convert.ToDateTime(txtEdad.Text) > DateTime.Now)
                                    fechavalidacion = false;
                            }
                            catch (System.Exception)
                            {
                                fechavalidacion = false;
                            }
                        }
                        #region Insertar datos y foto de usuario
                        if (fechavalidacion == true)
                        {
                            string url = "http://192.168.0.10:5001/api/Account/registrar";
                            string formato = "application/json";
                            usuarios usuarios = new usuarios()
                            {
                                nombre = txtUserName.Text,
                                email = txtEmail.Text,
                                contrasena = txtContrasena.Text,
                                id_dispositivo = Build.Serial,
                                SO = "Android",
                                tokenFB = "algo",
                                fecha_nacimiento = txtEdad.Text
                            };
                            var json = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, formato);
                            HttpClient cliente = new HttpClient();
                            cliente.Timeout = TimeSpan.FromSeconds(20);
                            if (actualizar != "1")
                            {
                                if (new ShareInside().HayConexion())
                                {
                                    progress.Visibility = Android.Views.ViewStates.Visible;
                                    Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                                    var respuesta = await cliente.PostAsync(url, json);
                                    contenido = await respuesta.Content.ReadAsStringAsync();
                                    respuesta.EnsureSuccessStatusCode();
                                    if (respuesta.IsSuccessStatusCode)
                                    {
                                        var cont = JsonConvert.DeserializeObject<Token>(contenido);
                                        if (cameraUri != null)
                                        {
                                            url_foto = await new ShareInside().SubirImagen("usuarios", Obtener_idusuario(cont.token), cameraUri);
                                            var server = "http://192.168.0.10:5001/api/Usuario/actualizarFoto?URL=" + url_foto;
                                            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cont.token);
                                            respuesta = await cliente.GetAsync(server);
                                            var content = await respuesta.Content.ReadAsStringAsync();
                                            if (respuesta.IsSuccessStatusCode)
                                            {
                                                Console.WriteLine("Datos Guardados");
                                            }
                                            else
                                            {
                                                Console.WriteLine("no se pudo actualizar la foto");
                                                url_foto = "foto";
                                            }
                                        }
                                        else
                                        {
                                            url_foto = "foto";
                                            Console.WriteLine("Campo imagen null");
                                        }
                                        usuarios.foto = url_foto;
                                        usuarios.id_usuario = Obtener_idusuario(cont.token);
                                        new ShareInside().GuardarToken(cont);
                                        new ShareInside().Guardar_DatosUsuario(usuarios);
                                        new ShareInside().Guardar_Email_Contrasena(txtEmail.Text, txtContrasena.Text);
                                        txtUserName.Text = "";
                                        txtEmail.Text = "";
                                        txtContrasena.Text = "";
                                        txtEdad.Text = "";
                                        progress.Visibility = Android.Views.ViewStates.Invisible;
                                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, contenido, ToastLength.Short).Show();
                                        progress.Visibility = Android.Views.ViewStates.Invisible;
                                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                                    }
                                }
                                else
                                    Toast.MakeText(this, "Tu conexion es inestable", ToastLength.Short).Show();
                            }
                            else
                            {
                               await ActualizarDatosUsuario(usuarios);
                            }
                        }
                        else
                        {
                            Toast.MakeText(this, "La fecha no es valida", ToastLength.Short).Show();
                        }
                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, contenido, ToastLength.Short).Show();
                    progress.Visibility = Android.Views.ViewStates.Invisible;
                    Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                }
            };
            if (actualizar=="1")
            {
                var datos = new ShareInside().Consultar_DatosUsuario();
                txtUserName.Text = datos.nombre;
                txtEmail.Text = datos.email;
                txtEmail.Enabled = false;
                txtEmail.Alpha = 0.5f;
                txtContrasena.Text = datos.contrasena;
                txtEdad.Text = datos.fecha_nacimiento;
                if (datos.foto == "")
                {
                    //foto 
                }
                else
                {
                    var descargarFoto = await new ShareInside().DownloadImageAsync(datos.foto,datos.id_usuario);
                    if (descargarFoto == "No hay conexion")
                    {
                        Toast.MakeText(this, GetText(Resource.String.No_internet_connection), ToastLength.Short).Show();
                    }
                    else
                        Foto.SetImageURI(Android.Net.Uri.Parse(descargarFoto));
                }

            }
        }

        private async Task ActualizarDatosUsuario(usuarios usuarios)
        {
            var contenido = "";
            try
            {
                var datos = new ShareInside().Consultar_DatosUsuario();
                string url = "http://192.168.0.10:5001/api/Usuario/actualizar";
                HttpClient cliente = new HttpClient();
                cliente.Timeout = TimeSpan.FromSeconds(20);
                if (new ShareInside().HayConexion())
                {
                    progress.Visibility = Android.Views.ViewStates.Visible;
                    Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                    if (cameraUri != null)
                    {
                        var url_foto = await new ShareInside().SubirImagen("usuarios", datos.id_usuario, cameraUri);
                    }
                    #region Actualizar usuario
                    usuarios.foto = datos.foto;
                    var json = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
                    var tok = await new ShareInside().ConsultarToken();
                    cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tok);
                    var respuesta = await cliente.PutAsync(url, json);
                    contenido = await respuesta.Content.ReadAsStringAsync();
                    //respuesta.EnsureSuccessStatusCode();
                    if (respuesta.IsSuccessStatusCode)
                    {
                        new ShareInside().Guardar_DatosUsuario(usuarios);
                        new ShareInside().Guardar_Email_Contrasena(usuarios.email, usuarios.contrasena);
                        progress.Visibility = Android.Views.ViewStates.Invisible;
                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        Console.WriteLine("Datos Guardados");
                    }
                    else
                    {
                        Console.WriteLine(contenido);
                        progress.Visibility = Android.Views.ViewStates.Invisible;
                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                    }
                    #endregion

                }
                else
                    Toast.MakeText(this, "Tu conexion es inestable", ToastLength.Short).Show();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(contenido);
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                progress.Visibility = Android.Views.ViewStates.Invisible;
                Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
            }
        }

        private void openCamara()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            crearRutaImagen();
            intent.PutExtra(MediaStore.ExtraOutput, cameraUri);
            intent.PutExtra("return data", true);
            StartActivityForResult(intent, camrequestcode);
        }

        public void crearRutaImagen()
        {
            Java.IO.File _dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CABASUS");

            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }

            Java.IO.File _file = new Java.IO.File(_dir, "Cabasus"
                + Java.Lang.String.ValueOf(JavaSystem.CurrentTimeMillis()) + ".jpg");

            cameraUri = Uri.FromFile(_file);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (requestCode == camrequestcode)
                    StartCropActivity(cameraUri);

                if (requestCode == REQUEST_SELECT_PICTURE)
                {
                    Uri selectedUri = data.Data;
                    if (selectedUri != null)
                    {
                        fotogaleria = ContentResolver.OpenInputStream(selectedUri);
                        Bitmap bitmap = BitmapFactory.DecodeStream(fotogaleria);

                        if (bitmap.Width < 100 | bitmap.Height < 100)
                        {
                            Android.App.AlertDialog.Builder mensajeError = new Android.App.AlertDialog.Builder(this);
                            mensajeError.SetTitle(GetText(Resource.String.Image_too_small));
                            mensajeError.SetMessage(GetText(Resource.String.image_with_192));
                            mensajeError.SetCancelable(false);
                            mensajeError.SetPositiveButton("Ok", this);
                            mensajeError.Show();
                        }
                        else
                        {
                            IsGalery = true;
                            StartCropActivity(data.Data);
                        }
                    }

                    else
                        Toast.MakeText(this, GetText(Resource.String.You_cant_recover_image), ToastLength.Short).Show();
                }
                else if (requestCode == UCrop.RequestCrop)
                {
                    HandleCropResult(data);
                }
                if (resultCode.ToString().Equals(UCrop.ResultError.ToString()))
                    HandleCropError(data);
            }
        }

        private void StartCropActivity(Uri uri)
        {

            try
            {
                string path = uri.Path.Substring(0, 37);
                string name = uri.Path.Substring(37, uri.Path.Length - 37);
                UCrop uCrop = UCrop.Of(uri, Uri.FromFile(new Java.IO.File(path, name))).WithAspectRatio(1, 1).WithMaxResultSize(1400, 1400);
                crop(uCrop);
            }
            catch (System.Exception)
            {
                string destinationFileName = "Cabasus.jpg";
                UCrop uCrop = UCrop.Of(uri, Uri.FromFile(new Java.IO.File(CacheDir, destinationFileName))).WithAspectRatio(1, 1).WithMaxResultSize(1400, 1400);
                crop(uCrop);
            }
        }

        public void crop(UCrop uCrop)
        {
            UCrop.Options options = new UCrop.Options();

            options.SetCompressionQuality(100);
            options.SetShowCropGrid(false);

            options.SetCompressionFormat(Bitmap.CompressFormat.Png);
            options.SetFreeStyleCropEnabled(false);
            options.SetToolbarColor(Color.Rgb(203, 30, 30));
            options.SetToolbarWidgetColor(Color.White);
            options.SetStatusBarColor(Color.Rgb(203, 30, 30));
            options.SetToolbarTitle(GetString(Resource.String.select_image));
            uCrop.WithOptions(options);

            uCrop.Start(this);
        }

        private void HandleCropResult(Intent result)
        {
            Uri resultUri = UCrop.GetOutput(result);
            if (resultUri != null)
            {
                fotogaleria = ContentResolver.OpenInputStream(resultUri);
                Bitmap bitmap = BitmapFactory.DecodeStream(fotogaleria);
                Foto.SetImageBitmap(bitmap);
                if (IsGalery)
                {
                    crearRutaImagen();
                    byte[] bitmapData;
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                        bitmapData = stream.ToArray();
                    }
                    File.WriteAllBytes(cameraUri.Path, bitmapData);
                }
            }
            else
            {
                Toast.MakeText(this, Resource.String.You_cant_recover_image, ToastLength.Short).Show();
            }
        }
        
        private void HandleCropError(Intent result)
        {
            Throwable cropError = UCrop.GetError(result);
            if (cropError != null)
            {
                System.Diagnostics.Debug.WriteLine(cropError.Message);
                Toast.MakeText(this, cropError.Message, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, Resource.String.The_information, ToastLength.Short).Show();
            }
        }

        private bool validar_email(EditText txt_email)
        {
            Regex email = new Regex(@"^([0-9a-zA-Z]" + //Start with a digit or alphabetical
                                           @"([\+\-_\.][0-9a-zA-Z]+)*" + // No continuous or ending +-_. chars in email
                                           @")+" +
                                           @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$");
            if (email.IsMatch(txt_email.Text))
                return true;
            else
                return false;
        }

        private string Obtener_idusuario(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "id").Value;
            return jti;
        }
    }
}