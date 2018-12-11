using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
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
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(203,30,30));
            gd.SetCornerRadius(500);
            txtIniciarSesion.SetBackgroundDrawable(gd);

            txtIniciarSesion.Click +=async delegate {
                try
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
                        StartActivity(typeof(ActivityPrincipal));
                        Finish();
                    }
                    else
                        Toast.MakeText(this, mensaje, ToastLength.Short).Show();


                }
                catch (Exception) { }
              
            };
        }
    }
}