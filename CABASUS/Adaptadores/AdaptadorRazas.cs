using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CABASUS.Modelos;

namespace CABASUS.Adaptadores
{
    public class AdaptadorRazas : BaseAdapter<Modelos.Razas>
    {
        List<Razas> items;
        Activity context;
        LayoutInflater li;
        Dialog dlg;
        TextView bds;
        public AdaptadorRazas(Activity context, List<Razas> items, Dialog d, TextView bre) : base()
        {
            this.context = context;
            this.items = items;
            dlg = d;
            bds = bre;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Razas this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var item = items[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.ItemRazas, null);
            view.FindViewById<TextView>(Resource.Id.lblraza).Text = item.raza;
            view.FindViewById<LinearLayout>(Resource.Id.linearraza).Click += delegate
            {
                dlg.Dismiss();
                bds.Tag = item.id_raza;
                bds.Text = item.raza;
            };
            return view;
        }
    }
}