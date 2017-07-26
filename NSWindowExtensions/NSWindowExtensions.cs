using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSWindowExtensions
{
	public static class NSWindowExtensions
	{
		public static Task<nint> RunSheetAsync(this AppKit.NSWindow owner, AppKit.NSWindow sheetWindow)
		{

			if (sheetWindow == null)
				throw new ArgumentNullException(nameof(sheetWindow));

			var tcs = new TaskCompletionSource<nint>();

			owner.BeginSheet(sheetWindow, result => tcs.SetResult(result));

			return tcs.Task;
		}

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

		public static void ShowAlertOnWindow(this AppKit.NSWindow owner, string title, string message, AppKit.NSAlertStyle style)
		{
			using (var alert = new AppKit.NSAlert())
			{
				alert.MessageText = title;
				alert.InformativeText = message;
				alert.AlertStyle = style;
				alert.BeginSheet(owner);
			}
		}

		public static Task<string> ShowSaveFileDialog(this AppKit.NSWindow owner, params string[] allowedExtension)
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

		public static Task<string> ShowOpenFileDialog(this AppKit.NSWindow owner, params string[] allowedExtension)
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

		public static Task<string> ShowOpenFileDialog(this AppKit.NSWindow owner)
		{
			var tcs = new TaskCompletionSource<string>();
			var sfd = AppKit.NSOpenPanel.OpenPanel;

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

		public static Task<string[]> SelectPathAsync(this AppKit.NSWindow owner, bool canChooseDir, bool canMultiSelection)
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
