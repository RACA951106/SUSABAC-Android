using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.Views;
using Android.Widget;
using CABASUS.Fragments;
using CABASUS.Modelos;

namespace CABASUS.Adaptadores
{
    public class AdaptadorCaballos : BaseAdapter<consultacompartidos>
    {
        private List<consultacompartidos> _listaCaballos;
        private FragmentHorses _fragmentHorses;
        List<string> _url_local;

        public AdaptadorCaballos(List<consultacompartidos> listaCaballos, List<string> url_local, FragmentHorses fragmentHorses)
        {
            _listaCaballos = listaCaballos;
            _fragmentHorses = fragmentHorses;
            _url_local = url_local;
        }

        public override consultacompartidos this[int position] { get { return _listaCaballos[position]; } }
        public override int Count { get { return _listaCaballos.Count; } }
        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _listaCaballos[position];
            var url_item = _url_local[position];
            View view = convertView;

            view = _fragmentHorses.LayoutInflater.Inflate(Resource.Layout.RowCaballos, null);
            var Foto = view.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.btnfotoCaballo);

            if (url_item == "No hay conexion")
                Foto.SetImageResource(Resource.Drawable.SetiBreed);
            else
                Foto.SetImageURI(Android.Net.Uri.Parse(url_item));

            return view;
        }
    }
}