using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace CABASUS.Fragments
{
    public class FragmentHorses : Fragment
    {
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.LayoutFragmentHorses, container, false);
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(203,30, 30));
            gd.SetCornerRadius(1000);

            var btnAgregar = Vista.FindViewById<ImageView>(Resource.Id.btnAdd);
            btnAgregar.SetBackgroundDrawable(gd);
            btnAgregar.Click += delegate {
                var intent = new Intent(Activity, typeof(Activity_RegistroCaballos));
                intent.PutExtra("ActuaizarCaballo", "false");
                intent.PutExtra("PrimerCaballo", "false");
                StartActivity(intent);
            };

            var ListaCaballos = new ShareInside().ConsultarCaballos();

            return Vista;
        }
    }
}