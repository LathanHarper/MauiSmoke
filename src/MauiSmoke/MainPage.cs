using System.Runtime.InteropServices;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;

namespace MauiSmoke;

public sealed class MainPage : ContentPage
{
    public MainPage()
    {
        Title = "MauiSmoke";
        BackgroundColor = Color.FromArgb("#071B2A");

        var stack = new VerticalStackLayout
        {
            Padding = new Thickness(28),
            Spacing = 16,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };

        stack.Children.Add(new Label
        {
            Text = "MAUISMOKE",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            CharacterSpacing = 4,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Color.FromArgb("#C7D7E3")
        });

        stack.Children.Add(new Label
        {
            Text = "BOOTED",
            FontSize = 52,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Color.FromArgb("#5EE6A8")
        });

        stack.Children.Add(new Label
        {
            Text = BuildLocalFacts(),
            FontSize = 16,
            LineHeight = 1.35,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Color.FromArgb("#FFFFFF")
        });

        stack.Children.Add(new Label
        {
            Text = "No network  •  No telemetry  •  No app data stored",
            FontSize = 13,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Color.FromArgb("#8FA9BA")
        });

        Content = new ScrollView
        {
            Content = stack
        };
    }

    private static string BuildLocalFacts()
    {
        var app = SafeRead(
            () => $"{AppInfo.Current.VersionString} ({AppInfo.Current.BuildString})",
            "unknown");
        var runtime = SafeRead(
            () => RuntimeInformation.FrameworkDescription,
            ".NET unknown");
        var platform = SafeRead(
            () => $"{DeviceInfo.Current.Platform} {DeviceInfo.Current.VersionString}",
            "iOS unknown");
        var device = SafeRead(
            () => $"{DeviceInfo.Current.Idiom} • {DeviceInfo.Current.Model}",
            "device unknown");

        return $"App {app}\nRuntime {runtime}\nPlatform {platform}\nDevice {device}";
    }

    private static string SafeRead(Func<string> read, string fallback)
    {
        try
        {
            var value = read();
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }
        catch
        {
            return fallback;
        }
    }
}
