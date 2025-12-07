
using System;
using System.Collections.Generic;
using static UpgradeSystem.Upgrade;

public static class UpgradeSystem
{
    public static float movementSpeedModifier { get; private set; } = 1f;
    public static float characterTiltModifier { get; private set; } = 1f;
    public static float pickpocketTimeExtension { get; private set; } = 0f;
    public static float pickpocketJiggleRate { get; private set; } = 1f;
    public static bool hasTreasureGlass { get; private set; } = false;

    public enum Upgrade
    {
        None,
        MovementSpeed,
        CharacterTilt,
        PickpocketTime,
        PickpocketJiggle,
        PickpocketTries,
        TreasureGlass
    }

    private static Dictionary<Upgrade, int> maxUpgrades = new Dictionary<Upgrade, int>
    {
        [None] = 0,
        [MovementSpeed] = 2,
        [CharacterTilt] = 3,
        [PickpocketTime] = 4,
        [PickpocketJiggle] = 2,
        [PickpocketTries] = 2,
        [TreasureGlass] = 1
    };

    public static Dictionary<Upgrade, int> upgradePrices { get; private set; } = new Dictionary<Upgrade, int>
    {
        [None] = 0,
        [MovementSpeed] = 3,
        [CharacterTilt] = 5,
        [PickpocketTime] = 4,
        [PickpocketJiggle] = 3,
        [PickpocketTries] = 4,
        [TreasureGlass] = 8
    };

    private static Dictionary<Upgrade, int> upgrades = new Dictionary<Upgrade, int>
    { };

    private static void RecalculateUpgrades()
    {
        movementSpeedModifier = 1f + Get(MovementSpeed) / 10f;
        characterTiltModifier = 1f + Get(CharacterTilt) / 8f;
        pickpocketTimeExtension = 5f * Get(PickpocketTime);
        pickpocketJiggleRate = 1f - Get(PickpocketJiggle) / 10f;
        hasTreasureGlass = Get(TreasureGlass) > 0 ? true : false;
    }

    // Resets all upgrades to level 0
    public static void Reset()
    {
        foreach (Upgrade up in (Upgrade[]) Enum.GetValues(typeof(Upgrade)))
        {
            upgrades[up] = 0;
        }
    }
    
    // Returns true if upgrade suceeded
    public static bool TryToUpgrade(Upgrade upgrade)
    {
        if (upgrades[upgrade] + 1 > maxUpgrades[upgrade]) return false;

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
