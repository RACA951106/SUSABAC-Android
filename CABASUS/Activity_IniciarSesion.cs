using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using CABASUS.Modelos;

namespace CABASUS
{
    [Activity(Label = "Activity_IniciarSesion")]
    public class Activity_IniciarSesion : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_IniciarSesion);
            Window.SetStatusBarColor(Color.Black);
            Window.SetNavigationBarColor(Color.Black);
            var txtCorreo = FindViewById<EditText>(Resource.Id.txtUserNameLogIn);
            var txtContrasena = FindViewById<EditText>(Resource.Id.txtContrasenaLogIn);
            var RecuperarContrasena = FindViewById<TextView>(Resource.Id.txtRecuperarContrasena);
            var txtIniciarSesion = FindViewById<TextView>(Resource.Id.btnIniciarSesionLogIn);
            var progress= FindViewById<ProgressBar>(Resource.Id.progressBar);
            progress.IndeterminateDrawable.SetColorFilter(Android.Graphics.Color.Rgb(203,30,30), Android.Graphics.PorterDuff.Mode.Multiply);

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(203,30,30));
            gd.SetCornerRadius(500);
            txtIniciarSesion.SetBackgroundDrawable(gd);

            txtIniciarSesion.Click +=async delegate {
                try
                {
                   
                    if (!string.IsNullOrWhiteSpace(txtContrasena.Text) && !string.IsNullOrWhiteSpace(txtCorreo.Text))
                    {
                        login log = new login()
                        {
                            usuario = txtCorreo.Text,
                            contrasena = txtContrasena.Text,
                            id_dispositivo = Build.Serial,
                            SO = "Android",
                            TokenFB = "algo"
                        };
                        progress.Visibility = Android.Views.ViewStates.Visible;
                        Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        var mensaje = await new ShareInside().LogearUsuario(log);
                        progress.Visibility = Android.Views.ViewStates.Invisible;
                        Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        if (mensaje == "Logeado")
                        {
                            var consultaCaballos = await new ConsumoAPIS().ConsultarCompartidos();
                            if (consultaCaballos == "No hay conexion")
                                Toast.MakeText(this, GetText(Resource.String.No_internet_connection), ToastLength.Short).Show();
                            else
                            {
                                if (bool.Parse(consultaCaballos))
                                {
                                    StartActivity(typeof(ActivityPrincipal));
                                    Finish();
                                }
                                else
                                {
                                    Intent intent = new Intent(this, (typeof(Activity_RegistroCaballos)));
                                    intent.PutExtra("ActuaizarCaballo", "false");
                                    intent.PutExtra("PrimerCaballo", "true");
                                    this.StartActivity(intent);
                                    Finish();
                                }
                            }
                        }
                        else
                            Toast.MakeText(this, mensaje, ToastLength.Short).Show();

                    }
                    else
                        Toast.MakeText(this, "Campos vacios", ToastLength.Short).Show();
                }
                catch (System.Exception) { }
            };

            RecuperarContrasena.Click += delegate {
                Dialog alertar = new Dialog(this, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.DialogoRecuperarContrasena);
                var btnSendPassword = alertar.FindViewById<TextView>(Resource.Id.btnSendPassword);
                var txtEmail = alertar.FindViewById<TextView>(Resource.Id.txtEmailRecuperarContrasena);
                GradientDrawable gdCreate = new GradientDrawable();
                gdCreate.SetColor(Color.Rgb(203, 30, 30));
                gdCreate.SetCornerRadius(500);
                btnSendPassword.SetBackgroundDrawable(gdCreate);
                btnSendPassword.Click += async delegate {
                    if (!string.IsNullOrEmpty(txtEmail.Text))
                    {
                        #region progress
                        ProgressBar progressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleLarge);
                        RelativeLayout.LayoutParams p = new RelativeLayout.LayoutParams(100, 100);
                        p.AddRule(LayoutRules.CenterInParent);
                        progressBar.IndeterminateDrawable.SetColorFilter(Color.Rgb(203,30,30), PorterDuff.Mode.Multiply);
                        alertar.FindViewById<RelativeLayout>(Resource.Id.dialogoRecuperarContrasena).AddView(progressBar, p);
                        progressBar.Visibility = Android.Views.ViewStates.Visible;
                        Window.AddFlags(Android.Views.WindowManagerFlags.NotTouchable);
                        await Task.Delay(500);
                        #endregion
                        var sendEmail = await new Modelos.ConsumoAPIS().RecuperarContrasena(txtEmail.Text, 1);
                        if (sendEmail == "No hay conexion")
                        {
                            progressBar.Visibility = Android.Views.ViewStates.Invisible;
                            Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                            Toast.MakeText(this, GetText(Resource.String.No_internet_connection), ToastLength.Short).Show();
                        }
                        else
                        {
                            if (bool.Parse(sendEmail))
                            {
                                progressBar.Visibility = Android.Views.ViewStates.Invisible;
                                Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                                alertar.Dismiss();
                                Toast.MakeText(this, GetText(Resource.String.New_password_sent), ToastLength.Short).Show();
                            }
                            else
                            {
                                progressBar.Visibility = Android.Views.ViewStates.Invisible;
                                Window.ClearFlags(Android.Views.WindowManagerFlags.NotTouchable);
                                Toast.MakeText(this, GetText(Resource.String.The_mail_could_not_be_sent), ToastLength.Short).Show();
                            }
                        }
                    }
                    else
                        Toast.MakeText(this, GetText(Resource.String.Empty_email_field), ToastLength.Short).Show();
                };
                alertar.Show();
            };
        }
    }
}