## BossBase Usage

To create a new boss, inherit from `BossBase`.

### Example

```csharp
public class DumpBoss : BossBase
{
    protected override void OnPhaseChange(int newPhase)
    {
        Debug.Log("Dump Boss changed to phase " + newPhase);

        // Change attack pattern or trigger animation here
    }

    protected override void OnDefeated()
    {
        Debug.Log("Dump Boss defeated!");

        base.OnDefeated();

        // Trigger boss-specific effects here
    }

    protected override void Die()
    {
        // Handle the boss death
    }
}