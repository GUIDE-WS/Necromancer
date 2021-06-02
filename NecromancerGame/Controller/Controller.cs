using System.Drawing;
using System.Linq;
using NecromancerGame.Model;

namespace NecromancerGame.Controller
{
    public static class Controller
    {
        public static void Interaction(Game game)
        {
            var player = game.CurrentLocation.CurrentNecromancer;
            var iPoint = DirectionWorker.CastDirectionToPoint(player.CurrentDirection);
            var doorPosition =
                new Point(iPoint.X + player.CurrentPosition.X, iPoint.Y + player.CurrentPosition.Y);
            if (!game.CurrentLocation.InBounds(doorPosition) ||
                !game.CurrentLocation.Doors.Contains(doorPosition))
                return;
            game.CurrentLocation.OpenDoor(doorPosition);
            if (doorPosition == game.CurrentLocation.Exit)
                game.ChangeLocation();
        }

        public static void SetPosition(Game game, Direction direction)
        {
            game.CurrentPlayer.MoveToDirection(direction);
        }


        public static void SetPlayer(Game game)
        {
            game.SetPlayer();
        }

        public static void Battle(IGameCharacter attacker, Location location)
        {
            var iPoint = DirectionWorker.CastDirectionToPoint(attacker.CurrentDirection);
            var targetPosition =
                new Point(iPoint.X + attacker.CurrentPosition.X, iPoint.Y + attacker.CurrentPosition.Y);
            var target = location.Enemies.FirstOrDefault(e => e.CurrentPosition == targetPosition);
            if (target == null && targetPosition == location.CurrentNecromancer.CurrentPosition)
                location.CurrentNecromancer.GetDamage(attacker.CurrentWeapon.Damage);
            else if (attacker != target)
                target?.GetDamage(attacker.CurrentWeapon.Damage);
        }
    }
}