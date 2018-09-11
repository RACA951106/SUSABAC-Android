using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using CABASUS.Adaptadores;
using Com.Gigamole.Infinitecycleviewpager;

namespace CABASUS.Fragments
{
    public class FragmentCycleHorses : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var Vista = inflater.Inflate(Resource.Layout.LayoutFragmentCycleHorses, container, false);

            HorizontalInfiniteCycleViewPager cycleViewPager = Vista.FindViewById<HorizontalInfiniteCycleViewPager>(Resource.Id.CycleViewPagerHorses);

            var Lista = new List<string>();
            for (int i = 0; i < 40; i++)
            {
                Lista.Add("dds");
            }

            AdaptadorCycleHorses AC = new AdaptadorCycleHorses(Lista, Context);
            cycleViewPager.Adapter = AC;

            return Vista;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}