using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace UWCLogo;

public partial class MainPage : ContentPage
{
    private LogoEngine engine = new OctagonShape();

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnExecuteClicked(object sender, EventArgs e)
    {
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
