## NSWindowExtensions
NSWindowExtensions that NSWindow Extensions for Xamarin.Mac

### Usage

```cs
public override ViewDidLoad()
{
    base.ViewDidLoad();
    SomeButton.Activated += async(sender , e) => await View.Window.RunAlertAsync("Welcome!","Hello Xamarin.Mac!"NSAlertStyle.Informational);
}
```
