using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSWindowExtensions
{
	public static class NSWindowExtensions
	{
        /// <summary>
        /// Runs the sheet async.
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
        /// Runs the alert async.
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
        /// Runs the confirm alert async.
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

			using (var alert = new AppKit.NSAlert())
			{
				alert.InformativeText = message;
				alert.MessageText = title;
				alert.AlertStyle = style;
				alert.AddButton("はい");
				alert.AddButton("いいえ");
				alert.BeginSheetForResponse(owner, ret => tcs.SetResult(ret == (int)AppKit.NSAlertButtonReturn.First));
			}
			return tcs.Task;
		}

        /// <summary>
        /// Shows the save file dialog.
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
        /// Shows the open file dialog.
        /// </summary>
        /// <returns>The open file dialog.</returns>
        /// <param name="owner">Owner.</param>
        /// <param name="allowedExtension">Allowed extension.</param>
		public static Task<string> ShowOpenFileDialogAsync(this AppKit.NSWindow owner, params string[] allowedExtension)
		{
			var tcs = new TaskCompletionSource<string>();
			var sfd = AppKit.NSOpenPanel.OpenPanel;
			sfd.AllowedFileTypes = allowedExtension;
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
        /// Shows the open file dialog.
        /// </summary>
        /// <returns>The open file dialog.</returns>
        /// <param name="owner">Owner.</param>
		public static Task<string[]> ShowOpenFileDialogAsync(this AppKit.NSWindow owner, bool canChooseDir, bool canMultiSelection)
		{
			var tcs = new TaskCompletionSource<string[]>();
			var panel = new AppKit.NSOpenPanel()
			{
				CanChooseDirectories = canChooseDir,
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
