public interface IDamageable
{
    public void GetDamage(float damage);
    public void Die();
    public float HealthMax { get; set; }
    public float HealthCurrent { get; set; }
}