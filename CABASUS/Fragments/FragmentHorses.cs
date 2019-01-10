using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CABASUS.Fragments
{
    public class FragmentHorses : Fragment
    {
        ListView ListViewCaballos;
        FragmentTransaction transaccion;
        List<string> selecccion = new List<string>();
        FragmentBarraBusqueda _FragmentBarraBusqueda = new FragmentBarraBusqueda();
        FragmentEliminarCaballo _FragmentEliminarCaballo;
        
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            await Task.Delay(1000);
            
            var ListaCaballos = new ShareInside().ConsultarCaballos();
            var ListaUrl_Local = await new ShareInside().ConsultarUrl_LocalAsync(ListaCaballos);
            ListViewCaballos.Adapter = new Adaptadores.AdaptadorCaballos(ListaCaballos, ListaUrl_Local, Activity, transaccion, _FragmentBarraBusqueda, _FragmentEliminarCaballo, selecccion);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.LayoutFragmentHorses, container, false);

            _FragmentEliminarCaballo = new FragmentEliminarCaballo(selecccion);

            transaccion = FragmentManager.BeginTransaction();
            transaccion.Add(Resource.Id.BarraBusqueda, _FragmentBarraBusqueda, "BusquedaCaballos");
            transaccion.Add(Resource.Id.BarraBusqueda, _FragmentEliminarCaballo, "EliminarCaballos");
            transaccion.Show(_FragmentBarraBusqueda);
            transaccion.Hide(_FragmentEliminarCaballo);
            transaccion.Commit();

            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(203, 30, 30));
            gd.SetCornerRadius(1000);
            var btnAgregar = Vista.FindViewById<ImageView>(Resource.Id.btnAdd);
            btnAgregar.SetBackgroundDrawable(gd);
            btnAgregar.Click += delegate {
                var intent = new Intent(Activity, typeof(Activity_RegistroCaballos));
                intent.PutExtra("ActuaizarCaballo", "false");
                intent.PutExtra("PrimerCaballo", "false");
                StartActivity(intent);
            };

            ListViewCaballos = Vista.FindViewById<ListView>(Resource.Id.lstCaballos);
            
            return Vista;
        }
    }
}