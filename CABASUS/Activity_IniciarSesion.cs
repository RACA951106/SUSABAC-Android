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

namespace CABASUS
{
    [Activity(Label = "Activity_IniciarSesion")]
    public class Activity_IniciarSesion : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_IniciarSesion);
            Window.SetStatusBarColor(Color.Rgb(246, 128, 25));
            Window.SetNavigationBarColor(Color.Rgb(246, 128, 25));

            var txtCorreo = FindViewById<EditText>(Resource.Id.txtUserNameLogIn);
            var txtContrasena = FindViewById<EditText>(Resource.Id.txtContrasenaLogIn);
            var RecuperarContrasena = FindViewById<TextView>(Resource.Id.txtRecuperarContrasena);
            var txtIniciarSesion = FindViewById<TextView>(Resource.Id.btnIniciarSesionLogIn);

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(246, 128, 25));
            gd.SetCornerRadius(500);
            txtIniciarSesion.SetBackgroundDrawable(gd);

            txtIniciarSesion.Click += delegate {
                StartActivity(typeof(ActivityPrincipal));
                Finish();
            };
        }
    }
}