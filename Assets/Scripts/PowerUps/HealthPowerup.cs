[System.Serializable]
public class HealthPowerup : PowerUp
{
    public float healthToChange;
    public override void Apply(PowerUpManager target)
    {
        // Apply health changes
        Health targetHealth = target.GetComponent<Health>();
        if(targetHealth != null)
        {
            // The second parameter is the pawn who caused the healing - in this case they healed themselves
            targetHealth.Heal(healthToChange, target.GetComponent<Pawn>());
        }
    }
    public override void Remove(PowerUpManager target)
    {
        // Remove health changes
        Health targetHealth = target.GetComponent<Health>();
        if(targetHealth != null)
        {
            targetHealth.TakeDamage(healthToChange, target.GetComponent<Pawn>());
        }
    }
}
