public interface IDamageable
{
    void TakeDamage(int amount, IEntity source);
    void Heal(int amount);
    int CurrentHealth { get; }
    int MaxHealth { get; }
}
