## NSWindowExtensions
This is NSWindow Extensions for Xamarin.Mac.
Nuget is [here](https://www.nuget.org/packages/NSWindowsExtensions/).

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
