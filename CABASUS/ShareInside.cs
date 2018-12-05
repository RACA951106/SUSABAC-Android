using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Lang;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using static Android.App.DatePickerDialog;

namespace CABASUS
{
   public class ShareInside
    {
        public bool HayConexion()
        {
            string CheckUrl = "https://www.google.com/";
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();
                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;
            }
            catch (WebException ex)
            {
                // Console.WriteLine (".....no connection..." + ex.ToString ());
                return false;
            }
        }

        public async Task<string> SubirImagen(string Contenedor, string Nombre, Android.Net.Uri RutaArchivo)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=susabac;AccountKey=p6EvYU5CRlr7U3nXEp0A+Q/M1ZRtReQjomO8EwaBJ00LxKoo/7MG/m7aX7pbdJGGcJ0HcYGzn6LM7lFYbMeR+g==;EndpointSuffix=core.windows.net");
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Contenedor);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Nombre + ".jpg");
            cloudBlockBlob.Properties.ContentType = "image/jpg";

            try
            {
                await cloudBlockBlob.UploadFromFileAsync(RutaArchivo.Path);
                return cloudBlockBlob.Uri.AbsoluteUri;
            }
            catch (System.Exception ex)
            {
                var mensaje = ex.Message;
                return "Error";
            }
        }
    }
    public class ObtenerDialogFecha
    {
        public class BaseActivity : AppCompatActivity
        {


            protected const int REQUEST_STORAGE_READ_ACCESS_PERMISSION = 101;
            protected const int REQUEST_STORAGE_WRITE_ACCESS_PERMISSION = 102;

            private Android.Support.V7.App.AlertDialog mAlertDialog;


            protected override void OnStop()
            {
                base.OnStop();
                if (mAlertDialog != null && mAlertDialog.IsShowing)
                {
                    mAlertDialog.Dismiss();
                }
            }

            protected void RequestPermission(string permission, string rationale, int requestCode)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permission))
                {
                    ShowAlertDialog("permission_title_rationale",
                                rationale,
                                (sender, args) =>
                                {
                                    ActivityCompat.RequestPermissions(this, new System.String[] { permission }, requestCode);
                                },
                                "ok",
                                null,
                                "cancel");
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new System.String[] { permission }, requestCode);
                }
            }

            protected void ShowAlertDialog(string title, string message, EventHandler<DialogClickEventArgs> onPositiveButtionClicked, string positiveButtonText, EventHandler<DialogClickEventArgs> onNegativeButtionClicked, string negativeButtonText)
            {
                Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);
                builder.SetTitle(title);
                builder.SetMessage(message);
                builder.SetPositiveButton(positiveButtonText, onPositiveButtionClicked);
                builder.SetNegativeButton(negativeButtonText, onNegativeButtionClicked);
                mAlertDialog = builder.Show();
            }
        }
        public class PickerDate : Java.Lang.Object, IOnDateSetListener
        {
            Button textoDate;
            TextView textoDateDiario;

            public PickerDate(Button textoDate)
            {
                this.textoDate = textoDate;
            }

            public PickerDate(TextView textoDateDiario)
            {
                this.textoDateDiario = textoDateDiario;
            }

            void IOnDateSetListener.OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
            {
                string m = (month + 1).ToString(), d = dayOfMonth.ToString();

                if (m.Length <= 1)
                    m = "0" + (month + 1).ToString();
                if (d.Length <= 1)
                    d = "0" + dayOfMonth.ToString();

                try { textoDate.Text = year.ToString() + "/" + m + "/" + d; } catch (System.Exception) { }
                try
                {
                    double Mes = (double.Parse(m) + 1);
                    if (Mes < 10)
                        m = "0" + Mes.ToString();
                    textoDateDiario.Text = d + "-" + m + "-" + year.ToString();
                }
                catch (System.Exception) { }
            }
        }
    }
    
}