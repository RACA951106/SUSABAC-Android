using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace CABASUS.Fragments
{
    public class FragmentHorses : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View Vista = inflater.Inflate(Resource.Layout.LayoutFragmentHorses, container, false);
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(246, 128, 25));
            gd.SetCornerRadius(1000);

            var btnAgregar = Vista.FindViewById<ImageView>(Resource.Id.btnAdd);
            btnAgregar.SetBackgroundDrawable(gd);
            btnAgregar.Click += delegate {
                var intent = new Intent(Activity, typeof(Activity_RegistroCaballos));
                intent.PutExtra("ActuaizarCaballo", "true");
                intent.PutExtra("PrimerCaballo", "false");
                StartActivity(intent);
            };
            return Vista;
        }
    }
}