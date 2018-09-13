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
        private List<string> TitulosViewPager;
        LayoutInflater layoutInflater;

        public PagerAdapterLogin(ActivityLogin activityLogin, List<string> datosViewPager, List<string> _TitulosViewPager, LayoutInflater _layoutInflater)
        {
            this.activityLogin = activityLogin;
            this.datosViewPager = datosViewPager;
            this.TitulosViewPager = _TitulosViewPager;
            this.layoutInflater = _layoutInflater;
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
            var root = layoutInflater.Inflate(Resource.Layout.layout_InfoLogin, null);
            var txtTitulo = root.FindViewById<TextView>(Resource.Id.txtTituloInfoLogin).Text = TitulosViewPager[position];
            var txtContenido = root.FindViewById<TextView>(Resource.Id.txtCuerpoInfoLogin).Text = datosViewPager[position];
            
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.AddView(root);

            return root;
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();

            viewPager.RemoveView(view as View);
        }
    }
}