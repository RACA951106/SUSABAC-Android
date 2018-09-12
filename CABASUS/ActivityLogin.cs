using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using System.Timers;
using Android.Media;

namespace CABASUS
{
    [Activity(Label = "ActivityLogin", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class ActivityLogin : Activity, MediaPlayer.IOnPreparedListener
    {
        ViewPager ViewPagerLogin;
        TabLayout TabViewPager;

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = true;
            mp.SetVolume(0, 0);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LayoutActivityLogin);

            var btnCreateAccount = FindViewById<TextView>(Resource.Id.btnCreateAccount);
            var btnLogIn = FindViewById<TextView>(Resource.Id.btnLogIn);
            var VideoFondo = FindViewById<VideoView>(Resource.Id.FondoVideo);

            VideoFondo.SetOnPreparedListener(this);
            //VideoFondo.ScaleX = 1.22f;
            //VideoFondo.ScaleY = 1.22f;
            var Ruta = "android.resource://CABASUS.CABASUS/" + Resource.Raw.FondoLogin;
            Android.Net.Uri uri = Android.Net.Uri.Parse(Ruta);
            VideoFondo.SetVideoURI(uri);
            VideoFondo.Start();

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(246, 128, 25));
            gd.SetCornerRadius(500);
            btnCreateAccount.SetBackgroundDrawable(gd);
            btnLogIn.SetBackgroundDrawable(gd);

            ViewPagerLogin = FindViewById<ViewPager>(Resource.Id.ViewPagerLogin);

            List<string> datosViewPager = new List<string> {"cuarto", "primero", "segundo", "tercero", "cuarto", "primero" };
            ViewPagerLogin.Adapter = new PagerAdapterLogin(this, datosViewPager);
            
            ViewPagerLogin.SetCurrentItem(1, false);

            ViewPagerLogin.PageScrolled += delegate
            {
                if (ViewPagerLogin.CurrentItem == 5)
                {
                    int cont = 0;
                    Timer T = new Timer();
                    T.Interval = 500;
                    T.Enabled = true;
                    T.Elapsed += (s, e) => {
                        RunOnUiThread(() => {
                            cont++;
                            if (cont == 1)
                            {
                                ViewPagerLogin.SetCurrentItem(1, false);
                                T.Stop();
                            }
                        });
                    };
                    T.Start();
                }
                if (ViewPagerLogin.CurrentItem == 0)
                {
                    int cont = 0;
                    Timer T = new Timer();
                    T.Interval = 500;
                    T.Enabled = true;
                    T.Elapsed += (s, e) => {
                        RunOnUiThread(() => {
                            cont++;
                            if (cont == 1)
                            {
                                ViewPagerLogin.SetCurrentItem(4, false);
                                T.Stop();
                            }
                        });
                    };
                    T.Start();
                }
            };
        }
    }
    public class PagerAdapterLogin : PagerAdapter
    {
        private ActivityLogin activityLogin;
        private List<string> datosViewPager;

        public PagerAdapterLogin(ActivityLogin activityLogin, List<string> datosViewPager)
        {
            this.activityLogin = activityLogin;
            this.datosViewPager = datosViewPager;
        }

        public override int Count
        {
            get { return datosViewPager.Count; }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(View container, int position)
        {
            var texto = new TextView(activityLogin);
            texto.Text = datosViewPager[position];
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.AddView(texto);
            
            return texto;
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();
            
            viewPager.RemoveView(view as View);
        }
    }
}