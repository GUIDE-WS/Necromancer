using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NecromancerGame.Model
{
    public class Game
    {
        public IGameCharacter CurrentPlayer { get; private set; }
        public Location CurrentLocation;
        private Location[] _locations;


        public Game()
        {
            _locations = LoadLocations().ToArray();
            CurrentLocation = _locations.First();
            CurrentPlayer = CurrentLocation.CurrentNecromancer;
        }

        public void Update()
        {
            CurrentLocation.Update();
            if (CurrentLocation.CurrentNecromancer.Health <= 0)
                RestartGame();
        }

        private static IEnumerable<Location> LoadLocations()
        {
            yield return new Location(Resources.Training);
            yield return new Location(Resources.ElfsForest);
            yield return new Location(Resources.SeveralRooms);
        }

        private void RouseGhost()
        {
            var position = FindBone(CurrentPlayer.CurrentPosition);
            if (position == null)
                return;
            CurrentLocation.AddGhost(position.Value);
        }

        private Point? FindBone(Point position)
        {
            for (var x = -1; x < 2; x++)
            for (var y = -1; y < 2; y++)
            {
                if (CurrentLocation.Map[x + position.X, y + position.Y] == MapElement.Bone)
                    return new Point(x + position.X, y + position.Y);
            }

            return null;
        }

        public void SetPlayer()
        {
            if (CurrentLocation.CurrentGhost == null)
                RouseGhost();
            if (CurrentPlayer == CurrentLocation.CurrentNecromancer && CurrentLocation.CurrentGhost != null)
                CurrentPlayer = CurrentLocation.CurrentGhost;
            else
                CurrentPlayer = CurrentLocation.CurrentNecromancer;
        }

        public void ChangeLocation()
        {
            var nextLocations = GetNextLocations().ToArray();
            if (!nextLocations.Any())
                return;
            _locations = nextLocations;
            CurrentLocation = _locations.First();
            CurrentPlayer = CurrentLocation.CurrentNecromancer;
        }

        private void RestartGame()
        {
            _locations = LoadLocations().ToArray();
            CurrentLocation = _locations.First();
            CurrentPlayer = CurrentLocation.CurrentNecromancer;
        }

        private IEnumerable<Location> GetNextLocations() 
            => _locations.Except(new[] {CurrentLocation}).ToArray();
    }
}