using System.Drawing;

namespace NecromancerGame.Model
{
    public static class DirectionWorker
    {
        public static Point CastDirectionToPoint(Direction direction)
        {
            var dx = 0;
            var dy = 0;
            switch (direction)
            {
                case Direction.Down:
                    dy = 1;
                    break;
                case Direction.Up:
                    dy = -1;
                    break;
                case Direction.Left:
                    dx = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    break;
            }

            return new Point(dx, dy);
        }
    }
}