
using System;
using System.Collections.Generic;
using static UpgradeSystem.Upgrade;

public static class UpgradeSystem
{
    public static int money = 0;

    private static float movementSpeedModifier = 1f;
    private static float characterTiltModifier = 1f;
    private static float pickpocketTimeExtension = 0f;
    private static bool hasTreasureGlass = false;

    public static float MovementSpeedModifier { get { return movementSpeedModifier; } }
    public static float CharacterTiltModifier { get { return characterTiltModifier; } }
    public static float PickpocketTimeExtension { get { return pickpocketTimeExtension; } }
    public static bool HasTreasureGlass { get { return hasTreasureGlass; } }

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
        [MovementSpeed] = 2,
        [CharacterTilt] = 3,
        [PickpocketTime] = 4,
        [TreasureGlass] = 1
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
        hasTreasureGlass = Get(TreasureGlass) > 0 ? true : false;
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
