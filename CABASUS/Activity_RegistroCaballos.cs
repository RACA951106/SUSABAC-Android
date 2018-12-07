using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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
using Newtonsoft.Json;
using Uri = Android.Net.Uri;

namespace CABASUS
{
    [Activity(Label = "Activity_RegistroCaballos")]
    public class Activity_RegistroCaballos : Activity
    {
        Refractored.Controls.CircleImageView Foto;
        private const int REQUEST_SELECT_PICTURE = 0x01;
        Uri cameraUri;
        int camrequestcode = 100;
        Stream fotogaleria;
        bool IsGalery = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            #region FindViewById Elementos generales
            SetContentView(Resource.Layout.layout_RegistrarCaballos);
            Window.SetStatusBarColor(Color.Rgb(246, 128, 25));
            Window.SetNavigationBarColor(Color.Rgb(246, 128, 25));

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

            txtDOB.TextChanged += delegate
            {
                new ShareInside().FormatoFecha(txtDOB);
            };
            txtDOB.Click += delegate { txtDOB.SetSelection(txtDOB.Text.Length); };
            //txtDOB.Focusable = false;
            txtDOB.MovementMethod = null;

            txtBreed.Click += delegate { };
            txtGender.Click += delegate { };
            txtListo.Click += async delegate {
                Modelos.caballos ModeloCaballos = new Modelos.caballos()
                {
                    nombre = "Francisco",
                    peso = 2.5,
                    altura = 3.6,
                    raza = 4,
                    fecha_nacimiento = "2018-05-06",
                    genero = 4,
                    avena = 10
                };
                try
                {
                    string id_caballo = await new Modelos.ConsumoAPIS().RegistrarCaballos(ModeloCaballos);
                    if (id_caballo == "No hay conexion")
                    {
                        Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                    }
                    else
                    {
                        string url_imagen = await new ShareInside().SubirImagen("caballos", id_caballo, cameraUri);
                        if (url_imagen == "No hay conexion")
                        {
                            Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                        }
                        else
                        {
                            Modelos.caballos FotoCaballo = new Modelos.caballos()
                            {
                                id_caballo = id_caballo,
                                foto = url_imagen
                            };
                            if (await new Modelos.ConsumoAPIS().ActualizarFotoCaballo(FotoCaballo) == "No hay conexion")
                            {
                                Toast.MakeText(this, "No hay conexion", ToastLength.Short).Show();
                            }
                            else
                            {
                                Toast.MakeText(this, "Datos del caballo guardados correctamente", ToastLength.Short).Show();
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
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
    }
}