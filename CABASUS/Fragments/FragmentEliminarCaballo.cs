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

namespace CABASUS.Fragments
{
    public class FragmentEliminarCaballo : Android.App.Fragment
    {
        private List<string> selecccion;

        public FragmentEliminarCaballo(List<string> selecccion)
        {
            this.selecccion = selecccion;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.layout_EliminarCaballos, container, false);
            var btnEliminar = Vista.FindViewById<ImageView>(Resource.Id.ImgEliminarCaballo);
            var btnActualizar = Vista.FindViewById<ImageView>(Resource.Id.imgActualizarCaballo);
            if (selecccion.Count == 1)
            {
                btnActualizar.Enabled = true;
            }
            else
            {
                btnActualizar.Enabled = false;
            }

            btnActualizar.Click += delegate {
                Toast.MakeText(Vista.Context, "Hola", ToastLength.Short).Show();
            };
            return Vista;
        }
    }
}