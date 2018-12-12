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

namespace CABASUS.Modelos
{
    public class consultacompartidos
    {
        public string id_caballo { get; set; }
        public string nombre_caballo { get; set; }
        public string foto_caballo { get; set; }
        public string id_usuario { get; set; }
        public string nombre_usuario { get; set; }
        public string foto_usuario { get; set; }
    }
}