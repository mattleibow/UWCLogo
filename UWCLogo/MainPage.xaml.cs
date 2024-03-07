using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace UWCLogo;

public partial class MainPage : ContentPage
{
    private readonly LogoEngine engine = new()
    {
    };

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnExecuteClicked(object sender, EventArgs e)
    {
        engine.Command =
            new GroupCommand(
                // down here
                new RepeatCommand(90,
                    new GroupCommand(
                        new ForwardCommand(1),
                        new RightCommand(1)
                    )
                ),
                // up here
                new PenUpCommand(),
                new RepeatCommand(90,
                    new GroupCommand(
                        new ForwardCommand(1),
                        new RightCommand(1)
                    )
                ),
                // down here
                new PenDownCommand(),
                new RepeatCommand(90,
                    new GroupCommand(
                        new ForwardCommand(1),
                        new RightCommand(1)
                    )
                ),
                // up here
                new PenUpCommand(),
                new RepeatCommand(90,
                    new GroupCommand(
                        new ForwardCommand(1),
                        new RightCommand(1)
                    )
                )
            );

        drawingSurface.InvalidateSurface();
    }

    private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var w = e.Info.Width;
        var h = e.Info.Height;

        engine.Draw(canvas, w, h);
    }
}
