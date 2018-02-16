## NSWindowExtensions
[NSWindowExtensions](https://www.nuget.org/packages/NSWindowsExtensions/) is NSWindow Extensions for Xamarin.Mac

## Install

```bash 
PM>Install-Package NSWindowsExtensions -Version 1.0.7.3
```

### Usage

```cs
public override ViewDidLoad()
{
    base.ViewDidLoad();
    SomeButton.Activated += async(sender , e) => await View.Window.RunAlertAsync("Welcome!","Hello Xamarin.Mac!"NSAlertStyle.Informational);
}
```
