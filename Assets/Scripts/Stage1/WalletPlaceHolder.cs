using UnityEngine;

public class WalletPlaceHolder : MonoBehaviour
{
    public static WalletPlaceHolder Instance { get; private set; }

    public int Coins { get; private set; } = 0;
    [SerializeField] private int startCoins = 8;
    [SerializeField] private int maxCoins = 10; //

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Coins = startCoins; //
        DontDestroyOnLoad(gameObject);
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }
}
