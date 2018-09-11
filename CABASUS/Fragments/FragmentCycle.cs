using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CABASUS.Adaptadores;
using Com.Gigamole.Infinitecycleviewpager;

namespace CABASUS.Fragments
{
    public class FragmentCycle : Fragment
    {
        List<string> listNombreCaballos;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var Vista = inflater.Inflate(Resource.Layout.LayoutFragmentCycleHorses, container, false);
            llenarListaCaballo();
            HorizontalInfiniteCycleViewPager cycleViewPager = (HorizontalInfiniteCycleViewPager)Vista.FindViewById(Resource.Id.horizontal_viewPager);
            AdaptadorHorses adaptadorHorse = new AdaptadorHorses(listNombreCaballos, Activity.BaseContext);
            cycleViewPager.Adapter = adaptadorHorse;

            cycleViewPager.PageScrolled += delegate {
                //Toast.MakeText(this, listNombreCaballos[cycleViewPager.RealItem], ToastLength.Short).Show();
                cycleViewPager.FindViewById<TextView>(Resource.Id.txtNombreCaballoPicker).Text = listNombreCaballos[cycleViewPager.RealItem];
            };


            // Use this to return your custom view for this Fragment
            return Vista;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        private void llenarListaCaballo()
        {
            listNombreCaballos = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                listNombreCaballos.Add("Caballo " + i);
            }
        }
    }
}