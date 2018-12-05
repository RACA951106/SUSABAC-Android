using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CABASUS
{
    [BroadcastReceiver(Name = "com.cabasus.app.CABASUS.VerificarInternet", Enabled = true)]
    [IntentFilter(new string[] { "android.net.conn.CONNECTIVITY_CHANGE" })]
   public class VerificarInternet : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var service = context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
            var networkinfo = service.ActiveNetworkInfo;
            if (networkinfo != null)
            {
                Toast.MakeText(context, "Internet", ToastLength.Short).Show();

            }
            else
            {
                 Toast.MakeText(context, "No Internet", ToastLength.Short).Show();
            }
        }
    }
}