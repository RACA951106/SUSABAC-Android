using System.Collections.Generic;
using Android.App;
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
        List<url_local> _url_local;
        FragmentTransaction _transaccion;
        FragmentBarraBusqueda _fragmentBarraBusqueda;
        FragmentEliminarCaballo _fragmentEliminarCaballo;
        List<string> Selecccion;
        public AdaptadorCaballos(List<consultacompartidos> listaCaballos, List<url_local> url_local, Activity fragmentHorses, FragmentTransaction transaccion, FragmentBarraBusqueda _FragmentBarraBusqueda, FragmentEliminarCaballo _FragmentEliminarCaballo, List<string> selecccion)
        {
            _listaCaballos = listaCaballos;
            _fragmentHorses = fragmentHorses;
            _url_local = url_local;
            _transaccion = transaccion;
            _fragmentBarraBusqueda = _FragmentBarraBusqueda;
            _fragmentEliminarCaballo = _FragmentEliminarCaballo;
            Selecccion = selecccion;
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
            view.FindViewById<TextView>(Resource.Id.txtNombreCaballo).Text = item.nombre_caballo;
            view.FindViewById<TextView>(Resource.Id.txtNombreUsuario).Text = _fragmentHorses.GetText(Resource.String.Owner) + ": " + item.nombre_usuario;
            try
            {
                if (url_item.foto_caballo == "No hay conexion")
                    Foto.SetImageResource(Resource.Drawable.SetiBreed);
                else
                    Foto.SetImageURI(Android.Net.Uri.Parse(url_item.foto_caballo));
            }
            catch (System.Exception)
            {
                Foto.SetImageResource(Resource.Drawable.SetiBreed);
            }

            view.LongClick += delegate {
                if (Selecccion.Contains(item.id_caballo))
                {
                    view.SetBackgroundColor(new Color(255, 255, 255));
                    Selecccion.RemoveAll(x => x == item.id_caballo);
                    if (Selecccion.Count != 0)
                    {
                        _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                        _transaccion.Remove(_fragmentEliminarCaballo);
                        _fragmentEliminarCaballo = new FragmentEliminarCaballo(Selecccion);
                        _transaccion.Add(Resource.Id.BarraBusqueda, _fragmentEliminarCaballo, "EliminarCaballos");
                        _transaccion.Hide(_fragmentBarraBusqueda);
                        _transaccion.Show(_fragmentEliminarCaballo);
                        _transaccion.Commit();
                    }
                    else
                    {
                        _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                        _transaccion.Show(_fragmentBarraBusqueda);
                        _transaccion.Hide(_fragmentEliminarCaballo);
                        _transaccion.Commit();
                    }
                }
                else
                {
                    view.SetBackgroundColor(new Color(209, 209, 209, 106));
                    Selecccion.Add(item.id_caballo);
                    _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                    _transaccion.Remove(_fragmentEliminarCaballo);
                    _fragmentEliminarCaballo = new FragmentEliminarCaballo(Selecccion);
                    _transaccion.Add(Resource.Id.BarraBusqueda, _fragmentEliminarCaballo, "EliminarCaballos");
                    _transaccion.Hide(_fragmentBarraBusqueda);
                    _transaccion.Show(_fragmentEliminarCaballo);
                    _transaccion.Commit();
                }
            };
            view.Click += delegate {
                if (Selecccion.Count != 0)
                {
                    if (Selecccion.Contains(item.id_caballo))
                    {
                        view.SetBackgroundColor(new Color(255, 255, 255));
                        Selecccion.RemoveAll(x => x == item.id_caballo);
                        if (Selecccion.Count != 0)
                        {
                            _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                            _transaccion.Remove(_fragmentEliminarCaballo);
                            _fragmentEliminarCaballo = new FragmentEliminarCaballo(Selecccion);
                            _transaccion.Add(Resource.Id.BarraBusqueda, _fragmentEliminarCaballo, "EliminarCaballos");
                            _transaccion.Hide(_fragmentBarraBusqueda);
                            _transaccion.Show(_fragmentEliminarCaballo);
                            _transaccion.Commit();
                        }
                        else
                        {
                            _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                            _transaccion.Show(_fragmentBarraBusqueda);
                            _transaccion.Hide(_fragmentEliminarCaballo);
                            _transaccion.Commit();
                        }
                    }
                    else
                    {
                        view.SetBackgroundColor(new Color(209, 209, 209, 106));
                        Selecccion.Add(item.id_caballo);
                        _transaccion = _fragmentHorses.FragmentManager.BeginTransaction();
                        _transaccion.Remove(_fragmentEliminarCaballo);
                        _fragmentEliminarCaballo = new FragmentEliminarCaballo(Selecccion);
                        _transaccion.Add(Resource.Id.BarraBusqueda, _fragmentEliminarCaballo, "EliminarCaballos");
                        _transaccion.Hide(_fragmentBarraBusqueda);
                        _transaccion.Show(_fragmentEliminarCaballo);
                        _transaccion.Commit();
                    }
                }
            };

            if (Selecccion.Contains(item.id_caballo))
            {
                view.SetBackgroundColor(new Color(209, 209, 209, 106));
            }
            

            return view;
        }
    }
}