using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NecromancerGame.Model
{
    public class Location
    {
        public Necromancer CurrentNecromancer { get; private set; }
        public Ghost CurrentGhost { get; private set; }
        public Point Exit { get; private set; }
        public MapElement[,] Map { get; private set; }

        public List<Point> Doors { get; private set; }

        public List<Enemy> Enemies { get; private set; }

        public Location(string textMap) => FromText(textMap);

        public bool InBounds(Point position)
        {
            var bounds = new Rectangle(0, 0, Map.GetLength(0), Map.GetLength(1));
            return bounds.Contains(position);
        }

        public bool IsPositionEmpty(Point position)
        {
            if (position == CurrentNecromancer.CurrentPosition)
                return false;
            return Enemies == null || Enemies.All(enemy => position != enemy.CurrentPosition);
        }

        public void AddGhost(Point position) => CurrentGhost = new Ghost(position.X, position.Y, this);

        public void Update()
        {
            Enemies = Enemies.Where(e => e.Health > 0).ToList();
            if(!Enemies.Any())
                return;
            foreach (var enemy in Enemies)
            {
                enemy.Update();
                Controller.Controller.Battle(enemy, this);
            }

        }
        
        public void OpenDoor(Point doorPosition)
        {
            if (Doors.Contains(doorPosition))
                Doors.Remove(doorPosition);
        }

        private void FromText(string text) =>
            FromLines(text.Split(new[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries));

        private void FromLines(string[] lines)
        {
            var map = new MapElement[lines[0].Length, lines.Length];
            var entry = Point.Empty;
            var exit = Point.Empty;
            var enemies = new List<Point>();
            var doors = new List<Point>();
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[0].Length; x++)
                {
                    switch (lines[y][x])
                    {
                        case '#':
                            map[x, y] = MapElement.Wall;
                            break;
                        case 'P':
                            map[x, y] = MapElement.Empty;
                            entry = new Point(x, y);
                            CurrentNecromancer = new Necromancer(x, y, this);
                            break;
                        case 'E':
                            map[x, y] = MapElement.Empty;
                            doors.Add(new Point(x,y));
                            exit = new Point(x, y);
                            break;
                        case 'D':
                            map[x, y] = MapElement.Empty;
                            doors.Add(new Point(x,y));
                            break;
                        case 'B':
                            map[x, y] = MapElement.Bone;
                            break;
                        case 'O':
                            map[x, y] = MapElement.Empty;
                            enemies.Add(new Point(x, y));
                            break;
                        default:
                            map[x, y] = MapElement.Empty;
                            break;
                    }
                }
            }

            Map = map;
            Doors = doors;
            Exit = exit;
            Enemies = enemies.Select(p => new Enemy(p.X, p.Y, this)).ToList();
        }
    }
}