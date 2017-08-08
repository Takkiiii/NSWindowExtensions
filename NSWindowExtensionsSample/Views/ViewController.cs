using System;
using AppKit;
using Foundation;
using NSWindowExtensions;

namespace NSWindowExtensionsSample.Views
{
    [Register(nameof(ViewController))]
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AlertButton.Activated += async (sender, e) => 
            {
                await View.Window.RunAlertAsync("Welcome!", "Xamarin.Mac World", NSAlertStyle.Informational);
            };
        }
    }
}
