## BossBase Usage

`BossBase` is the base class used for the bosses in the game. It handles
common boss functionality such as health, damage, phases and defeat events.
This means that each new boss does not need to create its own health and
phase system.

To create a new boss, create a new script that inherits from `BossBase`.

### Example

`DumpBoss` is an example of a boss that inherits from `BossBase`. The boss
can use the functionality from the base class and add its own behaviour
when phases change or when the boss is defeated.

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