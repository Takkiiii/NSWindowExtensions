using System;
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
        /// <param name="owner">Owner.</param>
        /// <param name="sheetWindow">Sheet window.</param>
		public static Task<nint> RunSheetAsync(this AppKit.NSWindow owner, AppKit.NSWindow sheetWindow)
		{
			if (sheetWindow == null)
				throw new ArgumentNullException(nameof(sheetWindow));
			var tcs = new TaskCompletionSource<nint>();
			owner.BeginSheet(sheetWindow, result => tcs.SetResult(result));
			return tcs.Task;
		}

        /// <summary>
        ///     Runs the alert async.
        /// </summary>
        /// <returns>The alert async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="style">Style.</param>
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
				alert.BeginSheetForResponse(owner, ret => tcs.SetResult(ret));
			}
			return tcs.Task;
		}

        /// <summary>
        ///     Runs the confirm alert async.
        /// </summary>
        /// <returns>The confirm alert async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="style">Style.</param>
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
				alert.BeginSheetForResponse(owner, ret => tcs.SetResult(ret == (int)AppKit.NSAlertButtonReturn.First));
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
        public static Task<string> ShowSaveFileDialogWithExtensionsPupUpButtonAsync(this AppKit.NSWindow owner, params string[] allowedExtension)
        {
            var tcs = new TaskCompletionSource<string>();
            var sfd = AppKit.NSSavePanel.SavePanel;
            sfd.CanCreateDirectories = true;
            var extensionsBox = new AppKit.NSPopUpButton(new CoreGraphics.CGRect(0, 0, 200, 24), false);
            extensionsBox.AddItems(allowedExtension);
			sfd.AccessoryView = extensionsBox;
			sfd.BeginSheet(owner, (result) =>
			{
				sfd.OrderOut(owner);
				if (result < 1)
					tcs.SetCanceled();
				else
                    tcs.SetResult($"{sfd.Url.Path}.{extensionsBox.SelectedItem.Title}");
			});
			return tcs.Task;
        }

        /// <summary>
        ///     Shows the open file dialog async.
        /// </summary>
        /// <returns>The open file dialog async.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="canChooseDir">If set to <c>true</c> can choose dir.</param>
        /// <param name="canMultiSelection">If set to <c>true</c> can multi selection.</param>
        /// <param name="allowedExtension">Allowed extension.</param>
		public static Task<string[]> ShowOpenFileDialogAsync(this AppKit.NSWindow owner, bool canChooseDir, bool canMultiSelection, params string[] allowedExtension)
		{
			var tcs = new TaskCompletionSource<string[]>();
			var panel = new AppKit.NSOpenPanel()
			{
				CanChooseDirectories = canChooseDir,
                AllowedFileTypes = allowedExtension,
				CanChooseFiles = !canChooseDir,
				AllowsMultipleSelection = canMultiSelection,
				CanCreateDirectories = !canMultiSelection,
				ReleasedWhenClosed = true
			};
			panel.BeginSheet(owner, ret => tcs.SetResult((ret > 0) ? panel.Urls.Select(x => x.Path).ToArray() : new string[] { }));
			return tcs.Task;
		}
	}
}
