using SkiaSharp;

namespace UWCLogo;

public class LogoEngine
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

    public void Draw(SKCanvas canvas, int w, int h)
    {
        currentCanvas = canvas;


        canvas.Clear(SKColors.White);

        canvas.Translate(w / 2, h / 2);
        canvas.Scale(3);


        // draw shape
        OnDraw();


        // draw origin and turtle
        DrawTurtle();


        currentCanvas = null;
    }

    protected virtual void OnDraw()
    {
    }

    public void Forward(double distance) => DrawLine(distance);
    
    public void Backward(double distance) => DrawLine(-distance);

    public void Right(double angle) => Rotate(angle);
    
    public void Left(double angle) => Rotate(-angle);

    private void DrawLine(double length)
    {
        var l = (float)length;

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
