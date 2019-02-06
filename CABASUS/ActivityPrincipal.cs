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
using Android.Support.V4.Widget;

namespace CABASUS
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class ActivityPrincipal : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        FragmentTransaction transaccion;
        FragmentAjustes _FragmentAjustes = new FragmentAjustes();

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.nav_diary)
            {
                
            }
            else if (id == Resource.Id.nav_activities)
            {
                
            }
            else if (id == Resource.Id.nav_calendar)
            {
            }
            else if (id == Resource.Id.nav_chat)
            {

            }
            else if (id == Resource.Id.nav_settings)
            {
                transaccion = FragmentManager.BeginTransaction();
                transaccion.Add(Resource.Id.FrameContent, _FragmentAjustes, "Ajustes");
                transaccion.Show(_FragmentAjustes);
                transaccion.Commit();
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Navigation);
            //Window.SetStatusBarColor(Color.Black);
            //Window.SetNavigationBarColor(Color.Black);

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            var btnMenu = FindViewById<ImageView>(Resource.Id.imgMenu);
            btnMenu.Click += delegate {
                drawer.OpenDrawer(GravityCompat.Start);
            };
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}