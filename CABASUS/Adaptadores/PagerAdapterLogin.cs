using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace CABASUS.Adaptadores
{
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