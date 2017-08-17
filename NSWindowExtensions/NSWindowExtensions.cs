using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;

namespace NSWindowExtensions
{
	public static class NSWindowExtensions
	{
        /// <summary>
        ///     Runs the sheet async.
        /// </summary>
        /// <returns>The sheet async.</returns>
        /// <param name="owner">Owner window.</param>
        /// <param name="sheetWindow">Sheet window.</param>
		public static Task<nint> RunSheetAsync(this AppKit.NSWindow owner, AppKit.NSWindow sheetWindow)
		{
			if (sheetWindow == null)
				throw new ArgumentNullException(nameof(sheetWindow));
			var tcs = new TaskCompletionSource<nint>();
			owner.BeginSheet(sheetWindow, result =>
            {
                sheetWindow.OrderOut(owner);
                tcs.SetResult(result);
            });
			return tcs.Task;
		}

        /// <summary>
        ///     Runs the modal.
        /// </summary>
        /// <returns>The modal.</returns>
        /// <param name="owner">Owner windwo.</param>
        /// <param name="sheetWindow">Sheet window.</param>
        public static AppKit.NSModalResponse RunModal(this AppKit.NSWindow owner, AppKit.NSWindow sheetWindow)
        {
			if (sheetWindow == null)
				throw new ArgumentNullException(nameof(sheetWindow));
            var ret = (AppKit.NSModalResponse) (int) AppKit.NSApplication.SharedApplication.RunModalForWindow(sheetWindow);
            sheetWindow.ContentViewController.View.Window.WillClose += (sender, e) => 
            { 
                AppKit.NSApplication.SharedApplication.StopModalWithCode((int)ret); 
            };
            return ret;
        }

        /// <summary>
        ///     Runs the alert async.
        /// </summary>
        /// <returns>The alert async.</returns>
        /// <param name="owner">Owner window.</param>
        /// <param name="title">Message's title.</param>
        /// <param name="message">Detile message.</param>
        /// <param name="style">NSAlert's style.</param>
		public static Task<nint> RunAlertAsync(this AppKit.NSWindow owner, string title, string message, AppKit.NSAlertStyle style)
		{
			if (owner == null)
				throw new ArgumentNullException(nameof(owner));
			var tcs = new TaskCompletionSource<nint>();
            using (var alert = new AppKit.NSAlert())
            {
                alert.InformativeText = message;
                alert.MessageText = title;
                alert.AlertStyle = style;
                var window = alert.Window;
                alert.BeginSheetForResponse(owner, ret =>
                {
                    window.OrderOut(owner);
                    tcs.SetResult(ret);
                });
            }
			return tcs.Task;
		}

		/// <summary>
		///     Runs the confirm alert async.
		/// </summary>
		/// <returns>The confirm alert async.</returns>
		/// <param name="owner">Owner.</param>
		/// <param name="title">Message title.</param>
		/// <param name="message">Detile message.</param>
		/// <param name="style">NSAlert's style.</param>
		public static Task<bool> RunConfirmAlertAsync(this AppKit.NSWindow owner, string title, string message, AppKit.NSAlertStyle style)
		{
			if (owner == null)
				throw new ArgumentNullException(nameof(owner));
			var tcs = new TaskCompletionSource<bool>();
            var locale = NSLocale.CurrentLocale;
			using (var alert = new AppKit.NSAlert())
			{
				alert.InformativeText = message;
				alert.MessageText = title;
				alert.AlertStyle = style;
                var window = alert.Window;
                if (locale.CollatorIdentifier == "ja-JP")
                {
                    alert.AddButton("はい");
                    alert.AddButton("いいえ");
                }
                else
                {
                    alert.AddButton("Yes");
                    alert.AddButton("No");
                }
				alert.BeginSheetForResponse(owner, ret =>
                {
                    window.OrderOut(null);
                    tcs.SetResult(ret == (int)AppKit.NSAlertButtonReturn.First);
                });
			}
			return tcs.Task;
		}

        /// <summary>
        ///     Shows the save file dialog.
        /// </summary>
        /// <returns>The save file dialog.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="allowedExtension">Allowed extension.</param>
		public static Task<string> ShowSaveFileDialogAsync(this AppKit.NSWindow owner, params string[] allowedExtension)
		{
			var tcs = new TaskCompletionSource<string>();
			var sfd = AppKit.NSSavePanel.SavePanel;
			sfd.AllowedFileTypes = allowedExtension;
			sfd.CanCreateDirectories = true;
			sfd.BeginSheet(owner, (result) =>
			{
				sfd.OrderOut(owner);
				if (result < 1)
					tcs.SetCanceled();
				else
					tcs.SetResult(sfd.Url.Path);
			});
			return tcs.Task;
		}

        /// <summary>
        ///     Shows the save file dialog with extensions pup up button async.
        /// </summary>
        /// <returns>The save file dialog with extensions pup up button async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="allowedExtension">Allowed extension.</param>
        public static Task<string> ShowSaveFileDialogWithExtensionsPupUpButtonAsync(this AppKit.NSWindow owner, Dictionary<string,string> allowedExtension)
        {
            var tcs = new TaskCompletionSource<string>();
            var panel = AppKit.NSSavePanel.SavePanel;
            panel.CanCreateDirectories = true;

            var accessoryView = new AppKit.NSView(new CoreGraphics.CGRect(0, 0, 270, 50));
            var extensionsBox = new AppKit.NSPopUpButton(new CoreGraphics.CGRect(130, 10, 120, 25), false);
            extensionsBox.AddItems(allowedExtension.Values.ToArray());
            var label = new AppKit.NSTextField(new CoreGraphics.CGRect(20, 15, 100, 18))
            {
                StringValue = "Format",
                Alignment = AppKit.NSTextAlignment.Right,
                Bordered = false,
                Selectable = false,
                Editable = false,
                BackgroundColor = AppKit.NSColor.Clear
            };

            accessoryView.AddSubview(label);
            accessoryView.AddSubview(extensionsBox);
            panel.AccessoryView = accessoryView;
			panel.BeginSheet(owner, (result) =>
			{
				panel.OrderOut(owner);
                if (result < 1)
                    tcs.SetCanceled();
                else
                {
                    var item = allowedExtension.Keys.ToArray()[(int)extensionsBox.IndexOfSelectedItem];
                    tcs.SetResult($"{panel.Url.Path}.{item}");
                }
			});
			return tcs.Task;
        }

        /// <summary>
        ///     Shows the open panel dialog async as slect folder panel.
        /// </summary>
        /// <returns>The open panel dialog async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="canChooseDir">If set to <c>true</c> can choose dir.</param>
        /// <param name="canMultiSelection">If set to <c>true</c> can multi selection.</param>
        public static Task<string[]> ShowOpenPanelDialogAsync(this AppKit.NSWindow owner, bool canChooseDir, bool canMultiSelection) => ShowOpenPanelDialogAsync(owner, canChooseDir, canMultiSelection, new[] { string.Empty });

        /// <summary>
        ///     Shows the open file dialog async.
        /// </summary>
        /// <returns>The open file dialog async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="canChooseDir">If set to <c>true</c> can choose dir.</param>
        /// <param name="canMultiSelection">If set to <c>true</c> can multi selection.</param>
        /// <param name="allowedExtension">Allowed extension.</param>
		public static Task<string[]> ShowOpenPanelDialogAsync(this AppKit.NSWindow owner, bool canChooseDir, bool canMultiSelection, params string[] allowedExtension)
		{
			var tcs = new TaskCompletionSource<string[]>();
			var panel = new AppKit.NSOpenPanel()
			{
				CanChooseDirectories = canChooseDir,
                AllowedFileTypes = allowedExtension,
				CanChooseFiles = !canChooseDir,
				AllowsMultipleSelection = canMultiSelection,
				CanCreateDirectories = !canMultiSelection,
				ReleasedWhenClosed = true,
			};
			panel.BeginSheet(owner, ret =>
            {
                panel.OrderOut(owner);
                tcs.SetResult((ret > 0) ? panel.Urls.Select(x => x.Path).ToArray() : null);
            });
			return tcs.Task;
		}
	}
}
