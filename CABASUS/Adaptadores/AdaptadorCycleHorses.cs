using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;

namespace CABASUS.Adaptadores
{
    class AdaptadorCycleHorses : PagerAdapter
    {
        List<string> CaballosID;
        LayoutInflater LayoutInflater;

        public override int Count => CaballosID.Count;

        public AdaptadorCycleHorses(List<string> Lista, Context context)
        {
            CaballosID = Lista;
            LayoutInflater = LayoutInflater.From(context);
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View Vista = LayoutInflater.Inflate(Resource.Layout.RowCycle, container, false);
            Vista.FindViewById<ImageView>(Resource.Id.ImagenCaballo).SetImageResource(Resource.Drawable.horse_icon);
            
            container.AddView(Vista);

            return Vista;
        }

        public override bool IsViewFromObject(View view, Object @object)
        {
            return view.Equals(@object);
        }
        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((View)@object);
        }
    }
}