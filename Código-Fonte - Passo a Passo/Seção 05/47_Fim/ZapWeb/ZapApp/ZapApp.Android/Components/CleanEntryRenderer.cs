using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZapApp.Components;
using ZapApp.Droid.Components;

[assembly:ExportRenderer(typeof(CleanEntry), typeof(CleanEntryRenderer))]
namespace ZapApp.Droid.Components
{
    public class CleanEntryRenderer : EntryRenderer
    {
        public CleanEntryRenderer(Context context) : base(context)
        {
            
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}