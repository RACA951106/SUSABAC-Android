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

namespace CABASUS.Adaptadores
{
    public class AdaptadorGender : BaseAdapter<string>
    {
        List<string> items;
        Activity context;
        Dialog dlg;
        TextView Gender;
        public AdaptadorGender(Activity context, List<string> items, Dialog d, TextView gender) : base()
        {
            this.context = context;
            this.items = items;
            dlg = d;
            Gender = gender;
        }

        public override Java.Lang.Object GetItem(int position) { return position; }
        public override long GetItemId(int position) { return position; }
        public override string this[int position] { get { return items[position]; } }
        public override int Count { get { return items.Count; } }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            view = context.LayoutInflater.Inflate(Resource.Layout.ItemGender, null);
            view.FindViewById<TextView>(Resource.Id.lblgender).Text = item;
            view.FindViewById<LinearLayout>(Resource.Id.leneargender).Click += delegate {
                dlg.Dismiss();
                Gender.Text = item;
                Gender.Tag = position+1;
            };
            return view;
        }
    }
}