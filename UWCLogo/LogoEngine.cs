using SkiaSharp;

namespace UWCLogo;

public interface ILogoEngine
{
    void Backward(double distance);

    void Forward(double distance);

    void Left(double angle);

    void Right(double angle);

    void PenDown();

    void PenUp();
}

public class LogoEngine : ILogoEngine
{
    private readonly SKPaint turlePaint = new()
    {
        Color = SKColors.Green,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 1
    };

    private readonly SKPaint linePaint = new()
    {
        Color = SKColors.Black,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 1
    };

    private readonly SKPath turtlePath = CreateTurtlePath();

    private SKCanvas? currentCanvas;

    private bool isPenDown = true;

    public LogoCommand? Command { get; set; }

    public void Draw(SKCanvas canvas, int w, int h)
    {
        currentCanvas = canvas;

        // setup canvas
        canvas.Clear(SKColors.White);
        canvas.Translate(w / 2, h / 2);
        canvas.Scale(3);

        // draw shape
        Command?.Execute(this);

        // draw origin and turtle
        DrawTurtle();

        currentCanvas = null;
    }

    // drawing commands

    public void Forward(double distance) => DrawLine(distance);

    public void Backward(double distance) => DrawLine(-distance);

    public void Right(double angle) => Rotate(angle);

    public void Left(double angle) => Rotate(-angle);

    public void PenDown() => isPenDown = true;

    public void PenUp() => isPenDown = false;

    // helpers

    private void DrawLine(double length)
    {
        var l = (float)length;

        if (isPenDown)
            currentCanvas?.DrawLine(0, 0, 0, -l, linePaint);

        currentCanvas?.Translate(0, -l);
    }

    private void Rotate(double angle)
    {
        var a = (float)angle;

        currentCanvas?.RotateDegrees(a);
    }

    private void DrawTurtle()
    {
        // draw origin
        currentCanvas?.DrawCircle(0, 0, 2, turlePaint);

        // draw triangle turtle
        currentCanvas?.DrawPath(turtlePath, turlePaint);
    }

    private static SKPath CreateTurtlePath()
    {
        var turtlePath = new SKPath();
        turtlePath.MoveTo(-17, 0);
        turtlePath.LineTo(0, -17);
        turtlePath.LineTo(17, 0);
        turtlePath.Close();
        return turtlePath;
    }
}
