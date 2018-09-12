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

namespace CABASUS
{
    [Activity(Label = "ActivityLogin", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class ActivityLogin : Activity
    {
        ViewPager ViewPagerLogin;
        TabLayout TabViewPager;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LayoutActivityLogin);

            var btnCreateAccount = FindViewById<TextView>(Resource.Id.btnCreateAccount);
            var btnLogIn = FindViewById<TextView>(Resource.Id.btnLogIn);

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(246, 128, 25));
            gd.SetCornerRadius(500);
            btnCreateAccount.SetBackgroundDrawable(gd);
            btnLogIn.SetBackgroundDrawable(gd);

            ViewPagerLogin = FindViewById<ViewPager>(Resource.Id.ViewPagerLogin);
            var PagerSeleccionado1 = FindViewById<LinearLayout>(Resource.Id.Seleccionado1);
            var PagerSeleccionado2 = FindViewById<LinearLayout>(Resource.Id.Seleccionado2);
            var PagerSeleccionado3 = FindViewById<LinearLayout>(Resource.Id.Seleccionado3);
            var PagerSeleccionado4 = FindViewById<LinearLayout>(Resource.Id.Seleccionado4);

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

                if (ViewPagerLogin.CurrentItem == 1)
                {
                    PagerSeleccionado1.SetBackgroundResource(Resource.Drawable.Selected_dot);
                    PagerSeleccionado2.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado3.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado4.SetBackgroundResource(Resource.Drawable.Default_dot);
                }
                else if (ViewPagerLogin.CurrentItem == 2)
                {
                    PagerSeleccionado1.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado2.SetBackgroundResource(Resource.Drawable.Selected_dot);
                    PagerSeleccionado3.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado4.SetBackgroundResource(Resource.Drawable.Default_dot);
                }
                else if (ViewPagerLogin.CurrentItem == 3)
                {
                    PagerSeleccionado1.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado2.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado3.SetBackgroundResource(Resource.Drawable.Selected_dot);
                    PagerSeleccionado4.SetBackgroundResource(Resource.Drawable.Default_dot);
                }
                else if (ViewPagerLogin.CurrentItem == 4)
                {
                    PagerSeleccionado1.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado2.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado3.SetBackgroundResource(Resource.Drawable.Default_dot);
                    PagerSeleccionado4.SetBackgroundResource(Resource.Drawable.Selected_dot);
                }
            };
            
            Timer TimerChangePage = new Timer();
            TimerChangePage.Interval = 3500;
            TimerChangePage.Enabled = true;
            TimerChangePage.Elapsed += (s, e) => {
                RunOnUiThread(() => {
                    if (ViewPagerLogin.CurrentItem == 1)
                        ViewPagerLogin.SetCurrentItem(2, true);
                    else if (ViewPagerLogin.CurrentItem == 2)
                        ViewPagerLogin.SetCurrentItem(3, true);
                    else if (ViewPagerLogin.CurrentItem == 3)
                        ViewPagerLogin.SetCurrentItem(4, true);
                    else if (ViewPagerLogin.CurrentItem == 4)
                        ViewPagerLogin.SetCurrentItem(1, false);
                });
            };
            TimerChangePage.Start();
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