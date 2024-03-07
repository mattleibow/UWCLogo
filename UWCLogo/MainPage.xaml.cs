using Microsoft.Maui.Controls.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Maui;

using UWCLogo.Engine;

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

    List<LogoCommand> commands = new List<LogoCommand>();

    private void OnExecuteClicked(object sender, EventArgs e)
    {
        try
        {
            var command = LogoCompiler.Compile(commandEntry.Text);
            commands.Add(command);

            engine.Command = new GroupCommand(commands.ToArray());
        }
        catch (Exception ex)
        {
            commandHistory.Text += ex.Message + Environment.NewLine;
        }

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
