// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NSWindowExtensionsSample.Views
{
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton AlertButton { get; set; }

		[Outlet]
		AppKit.NSButton SheetButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SheetButton != null) {
				SheetButton.Dispose ();
				SheetButton = null;
			}

			if (AlertButton != null) {
				AlertButton.Dispose ();
				AlertButton = null;
			}
		}
	}
}
