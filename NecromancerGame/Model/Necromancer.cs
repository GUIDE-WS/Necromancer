using System.Drawing;

namespace NecromancerGame.Model
{
    public class Necromancer : IGameCharacter
    {
        public int Health { get; private set; }
        public int MaxHealth { get; }
        private readonly Location _location;
        public Point CurrentPosition { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public IWeapon CurrentWeapon { get; }

        public Necromancer(int x, int y, Location location)
        {
            _location = location;
            CurrentWeapon = new Sword(25);
            MaxHealth = 150;
            Health = MaxHealth; ;
            CurrentPosition = new Point(x, y);
            CurrentSprite = Resources.NecromancerDown;
        }

        public Bitmap CurrentSprite { get; private set; }

        public void MoveToDirection(Direction direction)
        {
            CurrentDirection = direction;
            SetSprite();
            var positionIncrement = DirectionWorker.CastDirectionToPoint(direction);
            var newPosition = new Point(CurrentPosition.X + positionIncrement.X, CurrentPosition.Y + positionIncrement.Y);
            if (_location.InBounds(newPosition) && _location.IsPositionEmpty(newPosition) && IsCanMove(newPosition))
                CurrentPosition = newPosition;
        }

        private void SetSprite()
        {
            CurrentSprite = CurrentDirection switch
            {
                Direction.Up => Resources.NecromancerUp,
                Direction.Down => Resources.NecromancerDown,
                Direction.Left => Resources.NecromancerLeft,
                Direction.Right => Resources.NecromancerRight,
                _ => CurrentSprite
            };
        }

        public void GetDamage(int damage) => Health -= damage;

        private bool IsCanMove(Point newPosition) =>
            _location.Map[newPosition.X, newPosition.Y] == MapElement.Empty && !_location.Doors.Contains(newPosition);
    }
}