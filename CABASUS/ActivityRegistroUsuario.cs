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
    [Activity(Label = "ActivityRegistroUsuario")]
    public class ActivityRegistroUsuario : Activity
    {
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
            var txtTelefono = FindViewById<EditText>(Resource.Id.txtTelefonoRegistroUsuario);
            var Terminos = FindViewById<TextView>(Resource.Id.txtTerminosRegistroUsuario);
            var txtListo = FindViewById<TextView>(Resource.Id.btnListoRegistroUsuario);

            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(246, 128, 25));
            gdCreate.SetCornerRadius(500);
            txtListo.SetBackgroundDrawable(gdCreate);
        }
    }
}