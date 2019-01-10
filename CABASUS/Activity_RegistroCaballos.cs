using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Yalantis.Ucrop;
using Java.Lang;
using SQLite;
using Uri = Android.Net.Uri;
using CABASUS.Adaptadores;
using Android.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CABASUS.ObtenerDialogFecha;
using System.Net;
using System.Xml.Serialization;
using CABASUS.Modelos;
using System.Xml;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class Activity_RegistroCaballos : BaseActivity
    {
        Refractored.Controls.CircleImageView Foto;
        private const int REQUEST_SELECT_PICTURE = 0x01, camrequestcode = 100;
        Uri cameraUri;
        Stream fotogaleria;
        ListView textListView;
        EditText buscar;
        bool IsGalery = false, PrimerCaballo, ActuaizarCaballo;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            #region FindViewById Elementos generales
            SetContentView(Resource.Layout.layout_RegistrarCaballos);
            Window.SetStatusBarColor(Color.Black);
            Window.SetNavigationBarColor(Color.Black);
            #region variables de otro activity
            PrimerCaballo = bool.Parse(Intent.GetStringExtra("PrimerCaballo"));
            ActuaizarCaballo = bool.Parse(Intent.GetStringExtra("ActuaizarCaballo"));
            #endregion
            
            var txtListo = FindViewById<TextView>(Resource.Id.btnListoRegistroCaballos);
            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(246, 128, 25));
            gdCreate.SetCornerRadius(500);
            txtListo.SetBackgroundDrawable(gdCreate);
            Foto = FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.btnfotoCaballo);
            Drawable drawableImage = GetDrawable(Resource.Drawable.horse_icon);
            Bitmap bitDrawableImage = ((BitmapDrawable)drawableImage).Bitmap;
            Foto.SetImageBitmap(bitDrawableImage);
            var txtHorseName = FindViewById<EditText>(Resource.Id.txtHorseNameRegistro);
            var txtWeight = FindViewById<EditText>(Resource.Id.txtWeightRegistro);
            var txtHeight = FindViewById<EditText>(Resource.Id.txtHeightRegistro);
            var txtBreed = FindViewById<TextView>(Resource.Id.txtBreedRegistro);
            var txtDOB = FindViewById<EditText>(Resource.Id.txtDOBRegistro);
            var txtGender = FindViewById<TextView>(Resource.Id.txtGenderRegistro);
            var txtOat = FindViewById<EditText>(Resource.Id.txtOatRegistro);
            #endregion
            #region Clicks elementos
            Foto.Click += delegate
            {
                Dialog alertar = new Dialog(this, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(false);
                alertar.SetContentView(Resource.Layout.layout_CustomAlert);
                alertar.Show();
                alertar.FindViewById<Button>(Resource.Id.btnCancel).Click += delegate { alertar.Dismiss(); };
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
            txtDOB.TextChanged += delegate{ new ShareInside().FormatoFecha(txtDOB); };
            txtDOB.Click += delegate { txtDOB.SetSelection(txtDOB.Text.Length); };
            txtBreed.Click += delegate {
                Dialog alertar = new Dialog(this, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.DialogoRazas);
                var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RazasGender.sqlite"));
                var consulta = con.Query<Modelos.Razas>("select * from Razas", new Modelos.Razas().id_raza);
                textListView = alertar.FindViewById<ListView>(Resource.Id.ListaRazas);
                textListView.Adapter = new AdaptadorRazas(this, consulta, alertar, txtBreed);
                buscar = alertar.FindViewById<EditText>(Resource.Id.buscar);
                buscar.TextChanged += (object sender, TextChangedEventArgs e) =>
                {
                    var consulta2 = con.Query<Modelos.Razas>("select * from Razas where  raza like  '"+buscar.Text+"%'", new Modelos.Razas().id_raza);
                    textListView.Adapter = new AdaptadorRazas(this, consulta2, alertar, txtBreed);
                };
                alertar.Show();
            };
            txtGender.Click += delegate {
                Dialog alertar = new Dialog(this, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.DialogoGender);
                List<string> consulta = new List<string>() { GetText(Resource.String.Filly), GetText(Resource.String.Gelding), GetText(Resource.String.Mare), GetText(Resource.String.Stallion) };
                textListView = alertar.FindViewById<ListView>(Resource.Id.Listagender);
                textListView.Adapter = new AdaptadorGender(this, consulta, alertar, txtGender);
                alertar.Show();
            };
            txtListo.Click += async delegate {
                #region progress
                ProgressBar progressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleLarge);
                RelativeLayout.LayoutParams p = new RelativeLayout.LayoutParams(200, 200);
                p.AddRule(LayoutRules.CenterInParent);
                progressBar.IndeterminateDrawable.SetColorFilter(Color.Rgb(246, 128, 25), PorterDuff.Mode.Multiply);
                FindViewById<RelativeLayout>(Resource.Id.PantallaRegistroCaballos).AddView(progressBar, p);
                progressBar.Visibility = Android.Views.ViewStates.Visible;
                Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                await Task.Delay(500);
                #endregion
                if (string.IsNullOrEmpty(txtHorseName.Text) || string.IsNullOrEmpty(txtWeight.Text) || string.IsNullOrEmpty(txtHeight.Text) || string.IsNullOrEmpty(txtBreed.Text) || string.IsNullOrEmpty(txtDOB.Text) || string.IsNullOrEmpty(txtOat.Text))
                {
                    progressBar.Visibility = Android.Views.ViewStates.Invisible;
                    Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                    Toast.MakeText(this, Resource.String.There_are_empty_fields, ToastLength.Short).Show();
                }
                else
                {
                    try
                    {
                        Convert.ToDateTime(txtDOB.Text);
                        if (txtGender.Tag == null)
                            txtGender.Tag = 0;
                        Modelos.caballos ModeloCaballos = new Modelos.caballos()
                        {
                            nombre = txtHorseName.Text,
                            peso = double.Parse(txtWeight.Text),
                            altura = double.Parse(txtHeight.Text),
                            raza = int.Parse(txtBreed.Tag.ToString()),
                            fecha_nacimiento = Convert.ToDateTime(txtDOB.Text).ToString("yyyy-MM-dd"),
                            genero = int.Parse(txtGender.Tag.ToString()),
                            avena = int.Parse(txtOat.Text)
                        };
                        try
                        {
                            if (ActuaizarCaballo)
                                await ActualizacionCaballo(ModeloCaballos, progressBar);
                            else
                                await RegistrarCaballo(ModeloCaballos, progressBar);
                        }
                        catch (System.Exception ex)
                        {
                            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                            Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                        }
                    }
                    catch (System.Exception)
                    {
                        progressBar.Visibility = Android.Views.ViewStates.Invisible;
                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        Toast.MakeText(this, Resource.String.Incorrect_date_field, ToastLength.Short).Show();
                    }
                }
            };
            #endregion
            if (ActuaizarCaballo)
            {
                var datos_caballo = await new Modelos.ConsumoAPIS().ConsultarCaballo_Id("b80ebed26ac1454597376cacbe9993");
                if (datos_caballo.id_caballo == "No hay conexion")
                {
                    StartActivity(typeof(ActivityPrincipal));
                    Finish();
                    Toast.MakeText(this, GetText(Resource.String.No_internet_connection), ToastLength.Short).Show();
                }
                else
                {
                    txtHorseName.Text = datos_caballo.nombre;
                    txtWeight.Text = datos_caballo.peso.ToString();
                    txtHeight.Text = datos_caballo.altura.ToString();
                    var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RazasGender.sqlite"));
                    var consultaBreed = con.Query<Modelos.Razas>("select * from razas where id_raza = "+ datos_caballo.raza, new Modelos.Razas().id_raza);
                    txtBreed.Text = consultaBreed[0].raza;
                    txtBreed.Tag = consultaBreed[0].id_raza;
                    txtDOB.Text = Convert.ToDateTime(datos_caballo.fecha_nacimiento).ToString("dd / MM / yyyy");
                    if (datos_caballo.genero == 1)
                        txtGender.Text = GetText(Resource.String.Filly);
                    else if (datos_caballo.genero == 2)
                        txtGender.Text = GetText(Resource.String.Gelding);
                    else if (datos_caballo.genero == 3)
                        txtGender.Text = GetText(Resource.String.Mare);
                    else if (datos_caballo.genero == 4)
                        txtGender.Text = GetText(Resource.String.Stallion);
                    txtGender.Tag = datos_caballo.genero;
                    txtOat.Text = datos_caballo.avena.ToString();

                    if (datos_caballo.foto == "")
                    {
                        //foto 
                    }
                    else
                    {
                        var descargarFoto = await new ShareInside().DownloadImageAsync(datos_caballo.foto,"");
                        if (descargarFoto == "No hay conexion")
                        {
                            Toast.MakeText(this, GetText(Resource.String.No_internet_connection), ToastLength.Short).Show();
                        }
                        else
                            Foto.SetImageURI(Android.Net.Uri.Parse(descargarFoto));
                    }
                }
            }
        }

        public async Task RegistrarCaballo(Modelos.caballos ModeloCaballos, ProgressBar progressBar)
        {
            string id_caballo = await new Modelos.ConsumoAPIS().RegistrarCaballos(ModeloCaballos);
            if (id_caballo == "No hay conexion")
            {
                progressBar.Visibility = Android.Views.ViewStates.Invisible;
                Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
            }
            else
            {
                if (cameraUri != null)
                {
                    string url_imagen = await new ShareInside().SubirImagen("caballos", id_caballo, cameraUri);
                    if (url_imagen == "No hay conexion")
                    {
                        progressBar.Visibility = Android.Views.ViewStates.Invisible;
                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                    }
                    else
                    {
                        Modelos.caballos FotoCaballo = new Modelos.caballos()
                        {
                            id_caballo = id_caballo,
                            foto = url_imagen
                        };;
                        if (await new Modelos.ConsumoAPIS().ActualizarFotoCaballo(FotoCaballo) == "No hay conexion")
                        {
                            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                            Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                        }
                        else
                        {
                            var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RazasGender.sqlite"));
                            con.Query<Modelos.consultacompartidos>("insert into DescargarCaballos values ('" + id_caballo + "', '" + ModeloCaballos.nombre + "', '" + FotoCaballo.foto + "', '" + new ShareInside().Consultar_DatosUsuario().id_usuario + "', '" + new ShareInside().Consultar_DatosUsuario().nombre + "', '" + new ShareInside().Consultar_DatosUsuario().foto + "');", new Modelos.consultacompartidos().id_caballo);
                            var foto = await new ShareInside().DownloadImageAsync(FotoCaballo.foto, id_caballo);
                            con.Query<url_local>("insert into url_local values('" + id_caballo + "', '" + foto + "', '" + "no" + "')", new url_local().id_caballo);
                            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                            StartActivity(typeof(ActivityPrincipal));
                            Finish();
                        }
                    }
                }
                else
                {
                    var con = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RazasGender.sqlite"));
                    con.Query<Modelos.consultacompartidos>("insert into DescargarCaballos values ('" + id_caballo + "', '" + ModeloCaballos.nombre + "', '" + "No hay conexion" + "', '" + new ShareInside().Consultar_DatosUsuario().id_usuario + "', '" + new ShareInside().Consultar_DatosUsuario().nombre + "', '" + new ShareInside().Consultar_DatosUsuario().foto + "');", new Modelos.consultacompartidos().id_caballo);
                    con.Query<url_local>("insert into url_local values('" + id_caballo + "', '" + "No hay conexion" + "', '" + "no" + "')", new url_local().id_caballo);
                    progressBar.Visibility = Android.Views.ViewStates.Invisible;
                    Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                    Toast.MakeText(this, "Datos guardados con foto por default", ToastLength.Short).Show();
                }
            }
        }
        public async Task ActualizacionCaballo(Modelos.caballos ModeloCaballos, ProgressBar progressBar)
        {
            string id_caballo = await new Modelos.ConsumoAPIS().ActualizarCaballo(ModeloCaballos);
            if (id_caballo == "No hay conexion")
            {
                progressBar.Visibility = Android.Views.ViewStates.Invisible;
                Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
            }
            else
            {
                //if (cameraUri != null)
                //{
                //    string url_imagen = await new ShareInside().SubirImagen("caballos", id_caballo, cameraUri);
                //    if (url_imagen == "No hay conexion")
                //    {
                //        progressBar.Visibility = Android.Views.ViewStates.Invisible;
                //        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                //        Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                //    }
                //    else
                //    {
                //        Modelos.caballos FotoCaballo = new Modelos.caballos()
                //        {
                //            id_caballo = id_caballo,
                //            foto = url_imagen
                //        };
                //        if (await new Modelos.ConsumoAPIS().ActualizarFotoCaballo(FotoCaballo) == "No hay conexion")
                //        {
                //            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                //            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                //            Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                //        }
                //        else
                //        {
                //            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                //            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                //            StartActivity(typeof(ActivityPrincipal));
                //            Finish();
                //        }
                //    }
                //}
                //else
                //{
                //    progressBar.Visibility = Android.Views.ViewStates.Invisible;
                //    Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                //    Toast.MakeText(this, "Datos guardados con foto por default", ToastLength.Short).Show();
                //}
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
                            //mensajeError.SetPositiveButton("Ok", this);
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

        public override void OnBackPressed()
        {
            if (PrimerCaballo.Equals(false))
            {
                StartActivity(typeof(ActivityPrincipal));
                Finish();
            }
        }
    }
}