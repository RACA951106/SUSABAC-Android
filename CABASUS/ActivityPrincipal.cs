using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.View;
using com.refractored;
using CABASUS.Adaptadores;
using Android.Widget;
using CABASUS.Fragments;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Views;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityPrincipal : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        FragmentTransaction transaccion;
        FragmentHorses _FragmentHorses = new FragmentHorses();
        FragmentAjustes _FragmentAjustes = new FragmentAjustes();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LayoutActivityPrincipal);
            Window.SetStatusBarColor(Color.Black);
            Window.SetNavigationBarColor(Color.Black);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            transaccion = FragmentManager.BeginTransaction();
            transaccion.Add(Resource.Id.Pantallas, _FragmentHorses, "Caballos");
            transaccion.Add(Resource.Id.Pantallas, _FragmentAjustes, "Ajustes");
            transaccion.Hide(_FragmentAjustes);
            transaccion.Show(_FragmentHorses);
            transaccion.Commit();
        }
        public override void OnBackPressed()
        {
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    transaccion = FragmentManager.BeginTransaction();
                    transaccion.Hide(_FragmentAjustes);
                    transaccion.Show(_FragmentHorses);
                    transaccion.Commit();
                    return true;
                case Resource.Id.navigation_team_journal:
                    
                    return true;
                case Resource.Id.navigation_activities:
                    
                    return true;
                case Resource.Id.navigation_notifications:

                    return true;
                case Resource.Id.navigation_settings:
                    transaccion = FragmentManager.BeginTransaction();
                    transaccion.Hide(_FragmentHorses);
                    transaccion.Show(_FragmentAjustes);
                    transaccion.Commit();
                    return true;
            }
            
            return false;
        }
    }
}