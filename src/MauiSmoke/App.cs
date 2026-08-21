namespace MauiSmoke;

public sealed class App : Application
{
    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new MainPage())
        {
            Title = "MauiSmoke"
        };
}
