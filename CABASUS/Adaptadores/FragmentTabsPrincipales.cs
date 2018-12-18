using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace CABASUS.Adaptadores
{
    internal class FragmentTabsPrincipales : Fragment
    {
        public int position;

        public static FragmentTabsPrincipales NuevaInstancia(int position)
        {
            var f = new FragmentTabsPrincipales();
            var b = new Bundle();
            b.PutInt("position", position);
            f.Arguments = b;
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            position = Arguments.GetInt("position");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = null;
            switch (position)
            {
                case 0:
                    Vista = inflater.Inflate(Resource.Layout.layoutPagina1, container, false);
                    Vista.FindViewById<LinearLayout>(Resource.Id.linear).SetBackgroundColor(Color.Black);
                    break;
                case 1://Se ve CicleView
                    Vista = inflater.Inflate(Resource.Layout.layoutPagina1, container, false);
                    Vista.FindViewById<LinearLayout>(Resource.Id.linear).SetBackgroundColor(Color.Blue);
                    break;
                case 2:// Se ve CicleView
                    Vista = inflater.Inflate(Resource.Layout.layoutPagina1, container, false);
                    Vista.FindViewById<LinearLayout>(Resource.Id.linear).SetBackgroundColor(Color.Red);
                    break;
                case 3:
                    Vista = inflater.Inflate(Resource.Layout.layoutPagina1, container, false);
                    Vista.FindViewById<LinearLayout>(Resource.Id.linear).SetBackgroundColor(Color.BlueViolet);
                    break;
                case 4:
                    Vista = inflater.Inflate(Resource.Layout.layoutPagina1, container, false);
                    Vista.FindViewById<LinearLayout>(Resource.Id.linear).SetBackgroundColor(Color.White);
                    break;
            }

            return Vista;           
        }
    }
}