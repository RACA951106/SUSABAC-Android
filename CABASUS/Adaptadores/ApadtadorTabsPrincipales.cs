using Android.App;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using com.refractored;
using CABASUS.Fragments;

namespace CABASUS.Adaptadores
{
    class AdaptadorTabsPrincipales : FragmentPagerAdapter, ICustomTabProvider
    {
        Activity Actividad = null;
        
        public AdaptadorTabsPrincipales(Android.Support.V4.App.FragmentManager fm, Activity a) : base(fm) { Actividad = a; }

        public override int Count
        {
            get
            {
                return 5;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            Android.Support.V4.App.Fragment Vista = null;
            switch (position)
            {
                case 0:
                    Vista = new FragmentHorses();
                    break;
                case 1:
                    Vista = FragmentTabsPrincipales.NuevaInstancia(position);
                    break;
                case 2:
                    Vista = FragmentTabsPrincipales.NuevaInstancia(position);
                    break;
                case 3:
                    Vista = FragmentTabsPrincipales.NuevaInstancia(position);
                    break;
                case 4:
                    Vista = FragmentTabsPrincipales.NuevaInstancia(position);
                    break;
            }
            return Vista;
        }

        public View GetCustomTabView(ViewGroup parent, int position)
        {
            View Vista = Actividad.LayoutInflater.Inflate(Resource.Layout.CustomTab, parent, false);
            ImageView Icono = Vista.FindViewById<ImageView>(Resource.Id.Icon);

            switch (position)
            {
                case 0:
                    Icono.SetImageResource(Resource.Drawable.horse_icon);
                    //Texto.Text = "Hola 1";
                    break;
                case 1:
                    Icono.SetImageResource(Resource.Drawable.timeline_icon);
                    //Texto.Text = "Hola 1";
                    break;
                case 2:
                    Icono.SetImageResource(Resource.Drawable.map_icon);
                    //Texto.Text = "Hola 1";
                    break;
                case 3:
                    Icono.SetImageResource(Resource.Drawable.bell_icon);
                    //Texto.Text = "Hola 1";
                    break;
                case 4:
                    Icono.SetImageResource(Resource.Drawable.menu_icon);
                    //Texto.Text = "Hola 1";
                    break;
            }
            return Vista;
        }
    }
    }