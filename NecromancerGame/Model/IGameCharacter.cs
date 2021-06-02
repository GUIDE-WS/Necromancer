using System.Drawing;

namespace NecromancerGame.Model
{
    public interface IGameCharacter
    {
        Point CurrentPosition { get; }
        Direction CurrentDirection { get; }
        IWeapon CurrentWeapon { get; }
        void MoveToDirection(Direction direction);
    }

    public interface IWeapon
    {
        public int Damage { get; }
    }
    
    public class Sword : IWeapon
    {
        public Sword(int damage) => Damage = damage;

        public int Damage { get; }
        
    }
}