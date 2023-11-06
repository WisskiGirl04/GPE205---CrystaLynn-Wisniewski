public abstract class PowerUp 
{
    public float duration;
    public bool isPermanent;
    public abstract void Apply(PowerUpManager target);
    public abstract void Remove(PowerUpManager target);
}
