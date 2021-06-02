using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NecromancerGame.View
{
    public class PlayerBar : UserControl
    {
        private int Value { get; set; }
        private int MaxValue { get; }
        private int MinValue { get; }
        public Color BaseColor { get; init; }
        public Color ProgressColor { get; init; }

        public PlayerBar(int value, int maxValue, int minValue)
        {
            Value = value;
            MaxValue = maxValue;
            MinValue = minValue;
        }

        public void SetValue(int value)
        {
            if (value < MinValue || value > MaxValue)
                return;
            Value = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            var rectBase = new Rectangle(0, 0, Width - 1, Height - 1);
            var rectProgress = new Rectangle(
                rectBase.X,
                rectBase.Y,
                rectBase.Width * Value / (MaxValue - MinValue),
                rectBase.Height);


            DrawBase(g, rectBase);
            DrawProgress(g, rectProgress);
        }
        

        private void DrawProgress(Graphics g, Rectangle rect)
        {
            if (rect.Width <= 0)
                return;
            g.DrawRectangle(new Pen(ProgressColor), rect);
            g.FillRectangle(new SolidBrush(ProgressColor), rect);
        }

        private void DrawBase(Graphics g, Rectangle rect) => g.FillRectangle(new SolidBrush(BaseColor), rect);
    }
}