using System.Drawing;

namespace NecromancerGame.Model
{
    public class Necromancer : IGameCharacter
    {
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public Point CurrentPosition { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public IWeapon CurrentWeapon { get; }
        private readonly Location _location;

        public Necromancer(int x, int y, Location location)
        {
            _location = location;
            CurrentWeapon = new Sword(25);
            MaxHealth = 150;
            Health = MaxHealth; ;
            CurrentPosition = new Point(x, y);
            CurrentSprite = GameResources.NecromancerDown;
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

        public void GetDamage(int damage) => Health -= damage;

        private bool IsCanMove(Point newPosition) =>
            _location.Map[newPosition.X, newPosition.Y] == MapElement.Empty && !_location.Doors.Contains(newPosition);
        
        private void SetSprite()
        {
            CurrentSprite = CurrentDirection switch
            {
                Direction.Up => GameResources.NecromancerUp,
                Direction.Down => GameResources.NecromancerDown,
                Direction.Left => GameResources.NecromancerLeft,
                Direction.Right => GameResources.NecromancerRight,
                _ => CurrentSprite
            };
        }
    }
}