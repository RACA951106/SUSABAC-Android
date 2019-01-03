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
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
namespace CABASUS.Fragments
{
    public class FragmentAjustes : Android.App.Fragment
    {
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.layout_FragmentAjustes, container, false);

            #region Cuenta

            var btncuenta = Vista.FindViewById<LinearLayout>(Resource.Id.layoutacount);
            btncuenta.Click += delegate {
                var intent = new Intent(Activity, typeof(ActivityRegistroUsuario));
                intent.PutExtra("actualizar", "1");
                StartActivity(intent);
            };

            #endregion

            #region Acerca de 

           var btnacerca= Vista.FindViewById<LinearLayout>(Resource.Id.layoutaboutas);
            btnacerca.Click += delegate {
                Dialog alertar = new Dialog(Activity, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.DialogoAcercade);
                alertar.Show();
            };

            #endregion

            #region Terminos

            var terminos = Vista.FindViewById<LinearLayout>(Resource.Id.layouttermnos);
            terminos.Click += delegate {
                Dialog alertar = new Dialog(Activity, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.DialogoTermininos);
                var condiciones = alertar.FindViewById<TextView>(Resource.Id.txtliga);
                condiciones.PaintFlags |= PaintFlags.UnderlineText;
                condiciones.Click += delegate
                {
                    var uri = Android.Net.Uri.Parse("http://www.cabasus.com");
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                    alertar.Dismiss();
                };

                alertar.Show();
            };

            #endregion

            #region Cerrar sesion
            var logout = Vista.FindViewById<LinearLayout>(Resource.Id.layoutlogout);
            logout.Click += delegate {
                Android.App.Dialog alertar = new Android.App.Dialog(Activity, Resource.Style.Theme_Dialog_Translucent);
                alertar.RequestWindowFeature(1);
                alertar.SetCancelable(true);
                alertar.SetContentView(Resource.Layout.CustomAlertLogOut);
                alertar.FindViewById<TextView>(Resource.Id.btnYesClose).Click += delegate 
                {
                    try
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ConsultarEmail.xml"));
                        System.IO.File.Delete(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Token.xml"));
                        System.IO.File.Delete(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ListaCaballosPropiosYCompartidos.xml"));
                        System.IO.File.Delete(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatosUsuario.xml"));
                    }
                    catch (Exception) { }
                    var intent = new Intent(Activity, typeof(ActivityLogin));
                    StartActivity(intent);
                    Activity.Finish();
                };
                alertar.FindViewById<TextView>(Resource.Id.btnNoClose).Click += delegate
                {
                    alertar.Dismiss();
                };

                alertar.Show();
            };

            #endregion

            return Vista;
        }
    }
}