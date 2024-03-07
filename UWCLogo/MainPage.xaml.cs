using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace UWCLogo
{
    public partial class MainPage : ContentPage
    {
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

            canvas.Clear(SKColors.White);

            canvas.Translate(w / 2, h / 2);
            canvas.Scale(3);
            //canvas.RotateDegrees(45);

            var turlePaint = new SKPaint
            {
                Color = SKColors.Green,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };

            var linePaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };

            // draw shape

            for (int i = 0; i < 6; i++)
            {
                canvas.DrawLine(0, 0, 0, -100, linePaint);
                canvas.Translate(0, -100);

                canvas.RotateDegrees(45);
            }

            // draw origin and turtle

            canvas.DrawCircle(0, 0, 2, turlePaint);

            var turtlePath = new SKPath();
            turtlePath.MoveTo(-17, 0);
            turtlePath.LineTo(0, -17);
            turtlePath.LineTo(17, 0);
            turtlePath.Close();


            canvas.DrawPath(turtlePath, turlePaint);
        }
    }
}
