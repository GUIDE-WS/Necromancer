using System.Drawing;

namespace NecromancerGame.Model
{
    public class Ghost : IGameCharacter
    {
        public Point CurrentPosition { get; private set; }
        public Direction CurrentDirection { get; set; }
        public IWeapon CurrentWeapon { get; }
        public int Health { get; }
        public int MaxHealth { get; }
        private readonly Location _location;
        

        public Ghost(int x, int y, Location location)
        {
            Health = 10;
            MaxHealth = 10;
            _location = location;
            CurrentWeapon = new Sword(0);
            CurrentPosition = new Point(x, y);
            CurrentSprite = Resources.Ghost;
        }

        public Bitmap CurrentSprite { get; }

        public void MoveToDirection(Direction direction)
        {
            CurrentDirection = direction;
            var positionIncrement = DirectionWorker.CastDirectionToPoint(direction);
            var newPosition = new Point(CurrentPosition.X + positionIncrement.X, CurrentPosition.Y + positionIncrement.Y);
            if (_location.InBounds(newPosition))
                CurrentPosition = newPosition;
        }
    }
}