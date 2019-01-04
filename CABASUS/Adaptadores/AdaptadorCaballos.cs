using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using CABASUS.Fragments;
using CABASUS.Modelos;

namespace CABASUS.Adaptadores
{
    public class AdaptadorCaballos : BaseAdapter<consultacompartidos>
    {
        private List<consultacompartidos> _listaCaballos;
        private Activity _fragmentHorses;
        List<string> _url_local;
        FragmentTransaction _transaccion;
        FragmentBarraBusqueda _fragmentBarraBusqueda;
        FragmentEliminarCaballo _fragmentEliminarCaballo;
        int cantidad = 0;
        
        public AdaptadorCaballos(List<consultacompartidos> listaCaballos, List<string> url_local, Activity fragmentHorses, FragmentTransaction transaccion, FragmentBarraBusqueda _FragmentBarraBusqueda, FragmentEliminarCaballo _FragmentEliminarCaballo)
        {
            _listaCaballos = listaCaballos;
            _fragmentHorses = fragmentHorses;
            _url_local = url_local;
            _transaccion = transaccion;
            _fragmentBarraBusqueda = _FragmentBarraBusqueda;
            _fragmentEliminarCaballo = _FragmentEliminarCaballo;
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
            
            try
            {
                if (url_item == "No hay conexion")
                    Foto.SetImageResource(Resource.Drawable.SetiBreed);
                else
                    Foto.SetImageURI(Android.Net.Uri.Parse(url_item));
            }
            catch (System.Exception)
            {
                Foto.SetImageResource(Resource.Drawable.SetiBreed);
            }
            view.LongClick += delegate {
                if (cantidad>0)
                {
                    view.SetBackgroundColor(new Color(15, 20, 30));
                }
                else
                {
                    _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                    _transaccion.Hide(_fragmentBarraBusqueda);
                    _transaccion.Show(_fragmentEliminarCaballo);
                    _transaccion.Commit();
                    view.SetBackgroundColor(new Color(15, 20, 30));
                    cantidad++;
                }
            };
            view.Click += delegate {
                if (cantidad > 0)
                {
                    view.SetBackgroundColor(new Color(15, 20, 30));
                    cantidad++;
                }
            };

            return view;
        }
    }
}