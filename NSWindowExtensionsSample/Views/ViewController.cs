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
                await View.Window.RunAlertAsync("Welcome!", "Xamarin.Mac World", NSAlertStyle.Informational);
            };
            SheetButton.Activated += async (sender, e) => 
            {
                using(var viewController = InitiateAnotherViewController())
                {
                    var sheet = viewController.View.Window;
                    await View.Window.RunSheetAsync(sheet);
                    await View.Window.RunAlertAsync("Closed...", "Another view has closed.",NSAlertStyle.Informational);
                }
            };
            SelectDirectoryButton.Activated += async (sender, e) => 
            {
                try
                {
                    var path = await View.Window.ShowOpenPanelDialogAsync(true, false);
                    await View.Window.RunAlertAsync("Selected Directoy is ...", path[0], NSAlertStyle.Informational);
                }
                catch(OperationCanceledException)
                {
                    
                }
            };
            ModalButton.Activated += async (sender, e) => 
            {
				using (var viewController = InitiateAnotherViewController())
				{
					var sheet = viewController.View.Window;
                    View.Window.RunModal(sheet);
					await View.Window.RunAlertAsync("Closed...", "Another view has closed.", NSAlertStyle.Informational);
				}
            };
            SelectFileButton.Activated += async (sender, e) =>
            {
                var ret = await View.Window.ShowOpenPanelDialogAsync(false, false, new[] { "txt" });
                await View.Window.RunAlertAsync("Selected file is ...", ret[0], NSAlertStyle.Informational);
            };
            SelectFilesButton.Activated += async (sender, e) => 
            {
                try
                {
                    var ret = await View.Window.ShowOpenPanelDialogAsync(false, true, new[] { "txt" });
                    await View.Window.RunAlertAsync("Selected files are ...", string.Join("¥n", ret), NSAlertStyle.Informational);
                }
				catch (OperationCanceledException)
				{

				}
            };
            ConfirmButton.Activated += async (sender, e) => 
            {
                var ret = await View.Window.RunConfirmAlertAsync("Confirm", "Select Yes or No.", NSAlertStyle.Informational);
                await View.Window.RunAlertAsync("Selected button is ...", ret ? "Yes" : "No",NSAlertStyle.Informational);
            };
            SaveButton.Activated += async (sender, e) => 
            {
                try
                {
                    var path = await View.Window.ShowSaveFileDialogAsync(new[] { "txt" });
                    await View.Window.RunAlertAsync("File name is ...", path, NSAlertStyle.Informational);
                }
                catch(OperationCanceledException)
                {
                    
                }
            };
            SaveWith.Activated += async (sender, e) => 
            {
                try
                {
                    var path = await View.Window.ShowSaveFileDialogWithExtensionsPupUpButtonAsync(new System.Collections.Generic.Dictionary<string, string> { { "txt", "Text File (.txt)" }, { "xml", "XML File (.xml)" },{ "tif", "Credit image (.tif)" } });
                    await View.Window.RunAlertAsync("File name is ...", path, NSAlertStyle.Informational);
                }
				catch (OperationCanceledException)
				{

				}
            };
        }

        private AnotherViewController InitiateAnotherViewController()
        {
            var controller = Storyboard.InstantiateControllerWithIdentifier("AnotherView") as AppKit.NSWindowController;
            return controller.ContentViewController as AnotherViewController;
        }
    }
}
