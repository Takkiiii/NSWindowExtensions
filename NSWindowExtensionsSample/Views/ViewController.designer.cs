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
		AppKit.NSButton ConfirmButton { get; set; }

		[Outlet]
		AppKit.NSButton ModalButton { get; set; }

		[Outlet]
		AppKit.NSButton SaveButton { get; set; }

		[Outlet]
		AppKit.NSButton SaveWith { get; set; }

		[Outlet]
		AppKit.NSButton SelectDirectoryButton { get; set; }

		[Outlet]
		AppKit.NSButton SelectFileButton { get; set; }

		[Outlet]
		AppKit.NSButton SelectFilesButton { get; set; }

		[Outlet]
		AppKit.NSButton SheetButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SaveWith != null) {
				SaveWith.Dispose ();
				SaveWith = null;
			}

			if (AlertButton != null) {
				AlertButton.Dispose ();
				AlertButton = null;
			}

			if (ConfirmButton != null) {
				ConfirmButton.Dispose ();
				ConfirmButton = null;
			}

			if (ModalButton != null) {
				ModalButton.Dispose ();
				ModalButton = null;
			}

			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}

			if (SelectDirectoryButton != null) {
				SelectDirectoryButton.Dispose ();
				SelectDirectoryButton = null;
			}

			if (SelectFileButton != null) {
				SelectFileButton.Dispose ();
				SelectFileButton = null;
			}

			if (SelectFilesButton != null) {
				SelectFilesButton.Dispose ();
				SelectFilesButton = null;
			}

			if (SheetButton != null) {
				SheetButton.Dispose ();
				SheetButton = null;
			}
		}
	}
}
