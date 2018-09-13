using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using com.refractored;
using CABASUS.Adaptadores;
using Android.Widget;
using CABASUS.Fragments;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityPrincipal : AppCompatActivity
    {
        PagerSlidingTabStrip TabsPrincipales;
        ViewPager ViewPagerPrincipal;
        FrameLayout SelectorCaballos;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LayoutActivityPrincipal);

            TabsPrincipales = FindViewById<PagerSlidingTabStrip>(Resource.Id.TabsPrincipales);
            ViewPagerPrincipal = FindViewById<ViewPager>(Resource.Id.ViewPagerPrincipal);
            SelectorCaballos = FindViewById<FrameLayout>(Resource.Id.SelectorCaballos);

            ViewPagerPrincipal.Adapter = new AdaptadorTabsPrincipales(SupportFragmentManager, this);
            TabsPrincipales.SetViewPager(ViewPagerPrincipal);
            TabsPrincipales.GetChildAt(0).SetMinimumWidth(10);

            FragmentTransaction transaccion = FragmentManager.BeginTransaction();
            transaccion.Add(Resource.Id.SelectorCaballos, new FragmentCycleHorses());
            transaccion.Commit();

            ViewPagerPrincipal.PageScrolled += delegate 
            {
                var x = ViewPagerPrincipal.ScrollX;

                var metrics = Resources.DisplayMetrics;
                var width = metrics.WidthPixels;

                //regla de 3 para saber cuanto crecer 

                var Relacion = (x * 20f) / (float)width;

                if (Relacion < 20f)
                {
                    SelectorCaballos.LayoutParameters = new TableLayout.LayoutParams(-1, 0, Relacion);
                    ViewPagerPrincipal.LayoutParameters = new TableLayout.LayoutParams(-1, 0, 90f - Relacion);
                }
                if (Relacion >= 40f && Relacion <= 60f)  
                {
                    var RelacionInversa = 60f - Relacion;
                    SelectorCaballos.LayoutParameters = new TableLayout.LayoutParams(-1, 0, RelacionInversa);
                    ViewPagerPrincipal.LayoutParameters = new TableLayout.LayoutParams(-1, 0, 90f - RelacionInversa);
                }
            };


        }
    }
}