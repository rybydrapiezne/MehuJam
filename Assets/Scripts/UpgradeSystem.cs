
using System;
using System.Collections.Generic;
using static UpgradeSystem.Upgrade;

public static class UpgradeSystem
{
    public static int money = 0;

    public static float movementSpeedModifier = 1f;
    public static float characterTiltModifier = 1f;
    public static float pickpocketTimeExtension = 0f;

    public enum Upgrade
    {
        None,
        MovementSpeed,
        CharacterTilt,
        PickpocketTime,
        TreasureGlass
    }

    private static Dictionary<Upgrade, int> maxUpgrades = new Dictionary<Upgrade, int>
    {
        [None] = 0,
        [MovementSpeed] = 100,
        [CharacterTilt] = 20,
        [PickpocketTime] = 100,
        [TreasureGlass] = 100
    };

    private static Dictionary<Upgrade, int> upgrades = new Dictionary<Upgrade, int>
    {
        [None] = 0,
        [MovementSpeed] = 0,
        [CharacterTilt] = 0,
        [PickpocketTime] = 0,
        [TreasureGlass] = 0
    };

    private static void RecalculateUpgrades()
    {
        movementSpeedModifier = 1f + Get(MovementSpeed) / 10f;
        characterTiltModifier = 1f + Get(CharacterTilt) / 8f;
        pickpocketTimeExtension = 5f * Get(PickpocketTime);
    }

    // Resets all upgrades to level 0
    public static void SystemReset()
    {
        foreach (Upgrade up in (Upgrade[]) Enum.GetValues(typeof(Upgrade)))
        {
            upgrades[up] = 0;
        }
    }
    
    // Returns true if upgrade suceeded
    public static bool TryToUpgrade(Upgrade upgrade)
    {
        if (upgrades[upgrade] + 1 >= maxUpgrades[upgrade]) return false;

        upgrades[upgrade] += 1;

        RecalculateUpgrades();

        return true;
    }

    // Returns level of specified upgrade
    public static int Get(Upgrade upgrade)
    {
        return upgrades[upgrade];
    }
}
