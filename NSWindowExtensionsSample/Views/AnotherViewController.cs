using System;
using AppKit;
using Foundation;

namespace NSWindowExtensionsSample.Views
{
    [Register(nameof(AnotherViewController))]
    public partial class AnotherViewController : NSViewController
    {
        public AnotherViewController(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CloseButton.Activated += (sender, e) =>
            {
                NSApplication.SharedApplication.StopModalWithCode((int)NSModalResponse.Cancel);
                if (this.View.Window.SheetParent == null)
                    this.View.Window.Close();
                else
                    this.View.Window.SheetParent?.EndSheet(View.Window, 0);
            };
        }
    }
}
