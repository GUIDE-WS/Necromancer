using System;
using System.Drawing;
using System.Windows.Forms;

namespace NecromancerGame.View
{
    public class ScaledViewPanel : Panel
    {
        private readonly ScenePainter _painter;
        private PointF _centerLogicalPos;
        private float _zoomScale;

        public ScaledViewPanel(ScenePainter painter)
            : this() =>
            _painter = painter;
        
        protected override void InitLayout()
        {
            base.InitLayout();
            ResizeRedraw = true;
            DoubleBuffered = true;
        }
        

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            const float zoomChangeStep = 1.1f;
            switch (e.Delta)
            {
                case > 0:
                    ZoomScale *= zoomChangeStep;
                    break;
                case < 0:
                    ZoomScale /= zoomChangeStep;
                    break;
            }

            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_painter == null)
                return;
            var sceneSize = _painter.Size;
            if (FitToWindow)
            {
                var vMargin = sceneSize.Height * ClientSize.Width < ClientSize.Height * sceneSize.Width;
                _zoomScale = vMargin
                    ? ClientSize.Width / sceneSize.Width
                    : ClientSize.Height / sceneSize.Height;
                _centerLogicalPos = new PointF(sceneSize.Width / 2, sceneSize.Height / 2);
            }
            
            var shift = GetShift();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TranslateTransform(shift.X, shift.Y);
            e.Graphics.ScaleTransform(ZoomScale, ZoomScale);
            _painter.Paint(e.Graphics);
            base.OnPaint(e);
        }

        private ScaledViewPanel()
        {
            FitToWindow = true;
            _zoomScale = 1f;
        }

        private PointF CenterLogicalPos => _centerLogicalPos;

        private float ZoomScale
        {
            get => _zoomScale;
            set
            {
                _zoomScale = Math.Min(1000f, Math.Max(0.001f, value));
                FitToWindow = false;
            }
        }

        private bool FitToWindow { get; set; }

        private PointF GetShift()
        {
            return new(
                ClientSize.Width / 2f - CenterLogicalPos.X * ZoomScale,
                ClientSize.Height / 2f - CenterLogicalPos.Y * ZoomScale);
        }
    }
}