namespace SFMLTest.Entities
{
    public interface IProjectileOwner
    {
        void ProjectileDied(Projectile projectile);
        bool ValidateProjectileHit(Projectile projectile, Entity target);
        Vector2 Position { get; }
    }
}