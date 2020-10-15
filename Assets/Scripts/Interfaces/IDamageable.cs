namespace Interfaces
{
    // Enemys
    public interface IDamageable<T>
    {
        void TakeDamage(T damage);

    }

    // Only use on Turret to make damage over the enemy
    public interface IDamageableType<T>
    {
        void TakeDamage(T damage, DispenserData.Type damageType);
    }

}
