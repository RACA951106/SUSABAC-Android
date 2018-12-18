﻿using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CABASUS.Fragments
{
    public class FragmentHorses : Fragment
    {
        ListView ListViewCaballos;
        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var ListaCaballos = new ShareInside().ConsultarCaballos();
            List<string> url_local = new List<string>();
            if (File.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Url_FotosCaballos.xml")))
            {
                var serializador = new XmlSerializer(typeof(List<string>));
                var Lectura = new StreamReader(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Url_FotosCaballos.xml"));
                url_local = (List<string>)serializador.Deserialize(Lectura);
                Lectura.Close();
            }
            else
            {
                foreach (var item in ListaCaballos)
                {
                    url_local.Add(await new ShareInside().DownloadImageAsync(item.foto_caballo, item.id_caballo));
                }
                var serializador = new XmlSerializer(typeof(List<string>));
                var Escritura = new StreamWriter(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Url_FotosCaballos.xml"));
                serializador.Serialize(Escritura, url_local);
                Escritura.Close();
            }

            ListViewCaballos.Adapter = new Adaptadores.AdaptadorCaballos(ListaCaballos, url_local, this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View Vista = inflater.Inflate(Resource.Layout.LayoutFragmentHorses, container, false);
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(Color.Rgb(203,30, 30));
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