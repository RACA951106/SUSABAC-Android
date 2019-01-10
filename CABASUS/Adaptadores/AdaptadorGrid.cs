using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CABASUS;
using CABASUS.Modelos;
using FFImageLoading;
using Java.Lang;

namespace Galeria
{
    public class AdaptadorGrid : BaseAdapter<caballos>
    {
        List<caballos> lista;
        Activity context;
        DisplayMetrics metrics;
        FFImageLoading.Views.ImageViewAsync Foto;
        TextView nombre;
        RelativeLayout relative;
        public AdaptadorGrid(List<caballos> list, Activity activity, DisplayMetrics metri)
        {
            lista = list;
            context = activity;
            metrics = metri;
        }
        public override int Count
        {
            get { return lista.Count; }
        }
        
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override caballos this[int position]
        {
            get { return lista[position]; }
        }

        public override  View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = lista[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.ItemGrid, null);
            relative = view.FindViewById<RelativeLayout>(Resource.Id.relative);
            Foto = view.FindViewById<FFImageLoading.Views.ImageViewAsync>(Resource.Id.txttexto);
            nombre = view.FindViewById<TextView>(Resource.Id.txtnombre);
            nombre.Text = item.nombre;
            RelativeLayout.LayoutParams p1 = new RelativeLayout.LayoutParams((metrics.WidthPixels / 3)-5, (metrics.WidthPixels / 3)-5);
            relative.LayoutParameters = p1;
            ImageService.Instance.LoadUrl(item.foto).DownSample(width: 200).Into(Foto);
            return view;
        }
    }
  
}
