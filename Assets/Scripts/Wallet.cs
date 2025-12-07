using UnityEngine;

public static class Wallet
{

    public static int coins { get; private set; } = 0;
    [SerializeField] private static int startCoins = 8;
    [SerializeField] private static int maxCoins = 10; //

    public static void Reset()
    {
        coins = startCoins;
    }

    public static bool TrySpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        return false;
    }

    public static void AddCoins(int amount)
    {
        coins += amount;
    }
}
