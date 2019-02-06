using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.View;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using System.Timers;
using CABASUS.Adaptadores;
using Android.Media;
using System.IO;

namespace CABASUS
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
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
            Window.SetStatusBarColor(Color.Black);
            if (File.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Token.xml")))
            {
                StartActivity(typeof(ActivityPrincipal));
            }

            new ShareInside().CopyDocuments("RazasGender.sqlite", "RazasGender.db");

            #region Casamiento
            var btnCreateAccount = FindViewById<TextView>(Resource.Id.btnCreateAccount);
            var btnLogIn = FindViewById<TextView>(Resource.Id.btnLogIn);
            var VideoFondo = FindViewById<VideoView>(Resource.Id.FondoVideo);

            VideoFondo.SetOnPreparedListener(this);
            //VideoFondo.ScaleX = 1.22f;
            //VideoFondo.ScaleY = 1.22f;
            //var Ruta = "android.resource://CABASUS.CABASUS/" + Resource.Raw.FondoLogin;
            //Android.Net.Uri uri = Android.Net.Uri.Parse(Ruta);
            //VideoFondo.SetVideoURI(uri);
            //VideoFondo.Start();
            ViewPagerLogin = FindViewById<ViewPager>(Resource.Id.ViewPagerLogin);
            var PagerSeleccionado1 = FindViewById<LinearLayout>(Resource.Id.Seleccionado1);
            var PagerSeleccionado2 = FindViewById<LinearLayout>(Resource.Id.Seleccionado2);
            var PagerSeleccionado3 = FindViewById<LinearLayout>(Resource.Id.Seleccionado3);
            var PagerSeleccionado4 = FindViewById<LinearLayout>(Resource.Id.Seleccionado4);
            #endregion

            #region Estilo de botones
            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(203,30,30));
            gdCreate.SetCornerRadius(500);
            btnCreateAccount.SetBackgroundDrawable(gdCreate);

            GradientDrawable gdLogin = new GradientDrawable();
            gdLogin.SetColor(Color.Rgb(255, 255, 255));
            gdLogin.SetCornerRadius(500);
            btnLogIn.SetBackgroundDrawable(gdLogin);
            #endregion

            #region Contenido ViewPager
            List<string> TitulosViewPager = new List<string> {"Share", "Welcome", "Run", "History", "Share", "Welcome" };
            List<string> datosViewPager = new List<string> { "Info Share", "Info Welcome", "Info Run", "Info History", "Info Share", "Info Welcome" };
            ViewPagerLogin.Adapter = new PagerAdapterLogin(this, datosViewPager, TitulosViewPager, LayoutInflater);
            ViewPagerLogin.SetCurrentItem(1, false);
            #endregion

            #region ViewPager Infinito
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
            #endregion

            #region Cambiar paginas automaticamente
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
            #endregion

            #region crear cuenta
            btnCreateAccount.Click += delegate {
                StartActivity(typeof(ActivityRegistroUsuario));
            };
            #endregion

            #region iniciar sesion
            btnLogIn.Click += delegate {
                 StartActivity(typeof(ActivityPrincipal));
            };
            #endregion
        }
    }
}