using System;
using System.Collections.ObjectModel;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace TimeConverterDroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            TextView tvPacific = FindViewById<TextView>(Resource.Id.pacTime);
            TextView tvMountain = FindViewById<TextView>(Resource.Id.mtnTime);
            TextView tvCentral = FindViewById<TextView>(Resource.Id.ctrlTime);
            TextView tvEastern = FindViewById<TextView>(Resource.Id.eastTime);
            TextView tvUTC = FindViewById<TextView>(Resource.Id.utcTime);

            System.Timers.Timer Timer1 = new System.Timers.Timer();
            Timer1.Start();
            Timer1.Interval = 1000;
            Timer1.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                DateTime utcNow = DateTime.UtcNow;
                tvUTC.Text = utcNow.ToString("yyyy-MM-dd HH:mm:ss");

                TimeConverterOutput pacTime = TimeConverter.ConvertTime(utcNow, "America/Los_Angeles");
                TimeConverterOutput mtnTime = TimeConverter.ConvertTime(utcNow, "America/Boise");
                TimeConverterOutput ctlTime = TimeConverter.ConvertTime(utcNow, "America/Chicago");
                TimeConverterOutput estTime = TimeConverter.ConvertTime(utcNow, "America/New_York");

                RunOnUiThread(() =>
                {
                    setTimeView(tvPacific, pacTime);
                    setTimeView(tvMountain, mtnTime);
                    setTimeView(tvCentral, ctlTime);
                    setTimeView(tvEastern, estTime);
                });
                //Delete time since it will no longer be used.

            };
            Timer1.Enabled = true;


        }

        private void setTimeView(TextView tv, TimeConverterOutput tco)
        {
            if (tco.Success)
            {
                tv.Text = tco.TimeOutput.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                if (!tco.TimeZoneFound)
                {
                    tv.Text = "Invalid Time Zone Name!";
                }
                else
                {
                    tv.Text = tco.Exception.Message;
                }

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
	}
}

