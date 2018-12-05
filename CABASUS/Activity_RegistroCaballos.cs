using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CABASUS
{
    [Activity(Label = "Activity_RegistroCaballos")]
    public class Activity_RegistroCaballos : Activity
    {
        //PickerDate onDateSetListener, onDateSetListenerFin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_RegistrarCaballos);
            Window.SetStatusBarColor(Color.Rgb(246, 128, 25));
            Window.SetNavigationBarColor(Color.Rgb(246, 128, 25));

            var txtListo = FindViewById<TextView>(Resource.Id.btnListoRegistroCaballos);
            GradientDrawable gdCreate = new GradientDrawable();
            gdCreate.SetColor(Color.Rgb(246, 128, 25));
            gdCreate.SetCornerRadius(500);
            txtListo.SetBackgroundDrawable(gdCreate);

            var txtHorseName = FindViewById<EditText>(Resource.Id.txtHorseNameRegistro);
            var txtWeight = FindViewById<EditText>(Resource.Id.txtWeightRegistro);
            var txtHeight = FindViewById<EditText>(Resource.Id.txtHeightRegistro);
            var txtBreed = FindViewById<TextView>(Resource.Id.txtBreedRegistro);
            var txtDOB = FindViewById<TextView>(Resource.Id.txtDOBRegistro);
            var txtGender = FindViewById<TextView>(Resource.Id.txtGenderRegistro);
            var txtOat = FindViewById<EditText>(Resource.Id.txtOatRegistro);

            txtBreed.Click += delegate { };
            txtDOB.Click += delegate {
                //Java.Util.Calendar calendar = Java.Util.Calendar.Instance;
                //int year = calendar.Get(Java.Util.CalendarField.Year);
                //int month = calendar.Get(Java.Util.CalendarField.Month);
                //int day_of_month = calendar.Get(Java.Util.CalendarField.DayOfMonth);
                //DatePickerDialog dialog;

                //dialog = new DatePickerDialog(this, Resource.Style.ThemeOverlay_AppCompat_Dialog_Alert,
                //onDateSetListener, year, month, day_of_month);
                //dialog.Show();
            };
            txtGender.Click += delegate { };
            txtListo.Click += delegate { };
        }
    }
}