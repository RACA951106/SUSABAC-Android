using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace CABASUS.Adaptadores
{
    public class AdaptadorHorses : PagerAdapter
    {
        /*
         * Caballos UP
         */

        private List<string> _listNombreCaballo;
        private Context _context;
        LayoutInflater _inflater;

        public AdaptadorHorses(List<string> listNombreCaballos, Context context)
        {
            _listNombreCaballo = listNombreCaballos;
            _context = context;
            _inflater = LayoutInflater.From(_context);
        }

        public override int Count => _listNombreCaballo.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view.Equals(@object);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view = _inflater.Inflate(Resource.Layout.LayoutFragmentCycleHorses, container, false);
            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imgCaballoPicker);
            TextView editText = view.FindViewById<TextView>(Resource.Id.txtNombreCaballoPicker);

            imageView.SetImageResource(Resource.Drawable.horse_icon);
            //editText.Text = _listNombreCaballo[position];

            container.AddView(view);
            return view;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((View)@object);
        }
    }
}