public interface IDamageable<T>
{
    void TakeDamage(T damage);
    void TakeDamage(T damage, DispenserData.Type damageType);
}
