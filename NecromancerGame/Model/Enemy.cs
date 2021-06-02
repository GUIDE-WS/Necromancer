using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NecromancerGame.Model
{
    public class Enemy : IGameCharacter
    {
        public int Health { get; private set; }
        public int MaxHealth { get; }
        private readonly Location _location;
        public Point CurrentPosition { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public Bitmap CurrentSprite { get; private set; }
        public IWeapon CurrentWeapon { get; }
        private Point _previousTarget;
        private Point _currentTarget;
        private List<Direction> _pathToPlayer;

        public Enemy(int x, int y, Location location)
        {
            _location = location;
            MaxHealth = 100;
            Health = MaxHealth;
            CurrentWeapon = new Sword(25);
            CurrentPosition = new Point(x, y);
            CurrentSprite = Resources.ElfRiht;
            _currentTarget = _location.CurrentNecromancer.CurrentPosition;
            _pathToPlayer = FindShortestPathToTarget();
            Update();
        }

        public void MoveToDirection(Direction direction)
        {
            CurrentDirection = direction;
            SetSprite();
            var positionIncrement = DirectionWorker.CastDirectionToPoint(direction);
            var newPosition = new Point(CurrentPosition.X + positionIncrement.X,
                CurrentPosition.Y + positionIncrement.Y);
            if (_location.InBounds(newPosition) && _location.IsPositionEmpty(newPosition) && IsCanMove(newPosition))
                CurrentPosition = newPosition;
        }

        private void SetSprite()
        {
            CurrentSprite = CurrentDirection switch
            {
                Direction.Up => Resources.ElfUp,
                Direction.Down => Resources.ElfDown,
                Direction.Left => Resources.ElfLeft,
                Direction.Right => Resources.ElfRiht,
                _ => CurrentSprite
            };
        }

        public void GetDamage(int damage) => Health -= damage;

        public void Update()
        {
            _previousTarget = _currentTarget;
            _currentTarget = _location.CurrentNecromancer.CurrentPosition;
            if (_currentTarget != _previousTarget)
                _pathToPlayer = FindShortestPathToTarget();
            MoveToDirection(_pathToPlayer.LastOrDefault());
            if (_pathToPlayer.Any())
                _pathToPlayer.RemoveAt(_pathToPlayer.Count - 1);
        }


        private bool IsCanMove(Point newPosition) =>
            _location.Map[newPosition.X, newPosition.Y] == MapElement.Empty && !_location.Doors.Contains(newPosition);

        private List<Direction> FindShortestPathToTarget()
        {
            var defaultPath = GeneratePathWithBfs().FirstOrDefault();
            return defaultPath == null ? new List<Direction> {Direction.None} : ConvertPathToDirection(defaultPath);
        }


        private static List<Direction> ConvertPathToDirection(IEnumerable<Point> path)
        {
            if (path == null)
                return new List<Direction>(0);
            var first = new List<Point>(path);
            first.RemoveAt(first.Count - 1);
            return first.Zip(path.Skip(1),
                (f, s) =>
                {
                    var dx = s.X - f.X;
                    var dy = s.Y - f.Y;
                    Direction direction;
                    if (dx < 0)
                        direction = Direction.Right;
                    else if (dx > 0)
                        direction = Direction.Left;
                    else if (dy < 0)
                        direction = Direction.Down;
                    else
                        direction = Direction.Up;
                    return direction;
                }).ToList();
        }

        private IEnumerable<SinglyLinkedList<Point>> GeneratePathWithBfs()
        {
            const int viewRange = 6;
            var queue = new Queue<SinglyLinkedList<Point>>();
            var visited = new HashSet<Point> {CurrentPosition};
            var pointStart = new SinglyLinkedList<Point>(CurrentPosition);
            queue.Enqueue(pointStart);
            while (queue.Count != 0)
            {
                var currentPath = queue.Dequeue();
                var possiblePath = GetPossibleTrack(currentPath.Value)
                    .Where(p => !visited.Contains(p));
                if (_currentTarget == currentPath.Value && currentPath.Length < viewRange)
                    yield return currentPath;
                foreach (var point in possiblePath)
                {
                    queue.Enqueue(new SinglyLinkedList<Point>(point, currentPath));
                    visited.Add(point);
                }
            }
        }

        private IEnumerable<Point> GetPossibleTrack(Point currentPoint) =>
            GetPossibleDirections
                .Select(p => new Point(p.X + currentPoint.X, p.Y + currentPoint.Y))
                .Where(p => _location.InBounds(p) && IsCanMove(p));

        private static IEnumerable<Point> GetPossibleDirections
        {
            get
            {
                yield return new Point(0, -1);
                yield return new Point(0, 1);
                yield return new Point(-1, 0);
                yield return new Point(1, 0);
            }
        }
    }
}