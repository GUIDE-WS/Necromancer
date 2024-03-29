﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NecromancerGame.Model;

namespace NecromancerGame.View
{
    public class ScenePainter
    {
        public SizeF Size;
        private Size _levelSize;

        private readonly Game _game;

        private Location _currentLocation;

        private Bitmap _mapImage;

        private int _cellWidth;
        private int _cellHeight;

        public ScenePainter(GameForm form)
        {
            
            _game = form.CurrentGame;
            _currentLocation = _game.CurrentLocation;
            CreateMap();
        }

        public void Paint(Graphics g)
        {
            if (_currentLocation != _game.CurrentLocation)
            {
                _currentLocation = _game.CurrentLocation;
                CreateMap();
            }
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawLevel(g);
            DrawDoors(g);
            DrawCharacters(g);
            DrawPlayer(g);
            DrawGhost(g);
        }
        

        private void DrawGhost(Graphics graphics)
        {
            if (_currentLocation.CurrentGhost != null)
                graphics.DrawImage(_currentLocation.CurrentGhost.CurrentSprite,
                    new Rectangle(
                        _currentLocation.CurrentGhost.CurrentPosition.X,
                        _currentLocation.CurrentGhost.CurrentPosition.Y,
                        1,
                        1));
        }

        private void DrawPlayer(Graphics graphics)
        {
            if (_currentLocation.CurrentNecromancer.Health > 0)
                graphics.DrawImage(_currentLocation.CurrentNecromancer.CurrentSprite,
                    new Rectangle(
                        _currentLocation.CurrentNecromancer.CurrentPosition.X,
                        _currentLocation.CurrentNecromancer.CurrentPosition.Y,
                        1,
                        1));
        }

        private void DrawCharacters(Graphics g)
        {
            if (!_currentLocation.Enemies.Any())
                return;
            foreach (var character in _currentLocation.Enemies)
                g.DrawImage(character.CurrentSprite,
                    new Rectangle(
                        character.CurrentPosition.X,
                        character.CurrentPosition.Y,
                        1,
                        1));
        }
        
        
        private void DrawDoors(Graphics graphics)
        {
            if (!_currentLocation.Doors.Any())
                return;
            foreach (var door in _currentLocation.Doors)
                graphics.DrawImage(GameResources.Exit, new Rectangle(door.X, door.Y, 1, 1));
        }

        private void DrawLevel(Graphics graphics) => 
            graphics.DrawImage(_mapImage, new Rectangle(0, 0, _levelSize.Width, _levelSize.Height));


        private void CreateMap()
        {
            _levelSize = new Size(_currentLocation.Map.GetLength(0), _currentLocation.Map.GetLength(1));
            Size = new SizeF(_currentLocation.Map.GetLength(0), _currentLocation.Map.GetLength(1));
            _cellWidth = GameResources.Empty.Width;
            _cellHeight = GameResources.Empty.Height;
            _mapImage = new Bitmap(_levelSize.Width * _cellWidth, _levelSize.Height * _cellHeight);
            using var graphics = Graphics.FromImage(_mapImage);
            for (var x = 0; x < Size.Width; x++)
            {
                for (var y = 0; y < Size.Height; y++)
                {
                    var image = GameResources.Empty;
                    if (_currentLocation.Map[x, y] == MapElement.Wall)
                        image = GameResources.Wall;
                    if (_currentLocation.Map[x, y] == MapElement.Bone)
                        image = GameResources.Bone;
                    graphics.DrawImage(image, new Rectangle(x * _cellWidth, y * _cellHeight, _cellWidth, _cellHeight));
                }
            }
            DrawDoors(graphics);
            DrawCharacters(graphics);
            DrawPlayer(graphics);
        }
    }
}