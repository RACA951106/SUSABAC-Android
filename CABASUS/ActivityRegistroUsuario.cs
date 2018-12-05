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
using static Android.App.DatePickerDialog;
using static CABASUS.ObtenerDialogFecha;
using CABASUS.Modelos;
using System.Xml;
using Newtonsoft.Json;
using System.Net.Http;
using Android.Webkit;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityRegistroUsuario : BaseActivity, IDialogInterfaceOnClickListener
    {
        PickerDate onDateSetListener;
        public void OnClick(IDialogInterface dialog, int which)
        {
            throw new NotImplementedException();
        }

        Refractored.Controls.CircleImageView Foto;
        string id_usuario;
        #region Tomar Foto e Imagen Galeria

        int camrequestcode = 100;
        Uri cameraUri;
        bool IsGalery = false;
        private const string TAG = "SampleActivity";
        Stream fotogaleria;

        private const int REQUEST_SELECT_PICTURE = 0x01;
        private const string SAMPLE_CROPPED_IMAGE_NAME = "SampleCropImage";

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_RegistroUsuario);
            Window.SetStatusBarColor(Color.Rgb(246, 128, 25));
            Window.SetNavigationBarColor(Color.Rgb(246, 128, 25));

            var txtUserName = FindViewById<EditText>(Resource.Id.txtUserNameRegistro);
            var txtEmail = FindViewById<EditText>(Resource.Id.txtEmailRegistroUsuario);
            var txtContrasena = FindViewById<EditText>(Resource.Id.txtContrasenaRegistroUsuario);
            var txtEdad = FindViewById<TextView>(Resource.Id.txtEdadRegistroUsuario);
            var Terminos = FindViewById<TextView>(Resource.Id.txtTerminosRegistroUsuario);
            var btnaceptar = FindViewById<TextView>(Resource.Id.btnRegistroUsuario);
            Foto = FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.btnfoto);
            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(246, 128, 25));
            gdCreate.SetCornerRadius(500);
            btnaceptar.SetBackgroundDrawable(gdCreate);

            Drawable drawableImage = GetDrawable(Resource.Drawable.horse_icon);
            Bitmap bitDrawableImage = ((BitmapDrawable)drawableImage).Bitmap; 

            Foto.SetImageBitmap(bitDrawableImage);

            #region Obtener edad
            txtEdad.Click += delegate
            {
                Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
                int year = calendar.Get(Java.Util.CalendarField.Year);
                int month = calendar.Get(Java.Util.CalendarField.Month);
                int day_of_month = calendar.Get(Java.Util.CalendarField.DayOfMonth);

                DatePickerDialog dialog = new DatePickerDialog(this, Resource.Style.ThemeOverlay_AppCompat_Dialog_Alert,
                   onDateSetListener, year, month, day_of_month);

                dialog.DatePicker.MaxDate = JavaSystem.CurrentTimeMillis();

                dialog.Show();
            };
            onDateSetListener = new PickerDate(txtEdad);
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
                try
                {
                     string url_imagen = await new ShareInside().SubirImagen("usuarios", "usuarioandroid", cameraUri);
                    Toast.MakeText(this, url_imagen, ToastLength.Long).Show();
                }
                catch (System.Exception)
                {

                    throw;
                }
            };
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

    }
}