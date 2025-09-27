using UnityEngine;
using UnityEngine.UI;

public class DebugScipt : MonoBehaviour
{
    public Text debugText;
    private DiceThrowerScript diceThrower;
    private PlayerTurn playerTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // diceThrowerScript = FindFirstObjectByType<DiceThrowerScript>();
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (PlayerTurn.Instance != null)
        // {
        //     int p1Money = PlayerTurn.Instance.P1Money;
        //     int p2Money = PlayerTurn.Instance.P2Money;
        // }
        int p1Money = PlayerTurn.Instance.P1Money;
        int p2Money = PlayerTurn.Instance.P2Money;
        // Cheats
        // bool? p1Cheat = diceThrower.GetLatestCheat(1);
        // bool? p2Cheat = diceThrower.GetLatestCheat(2);
        bool p1Cheat = diceThrower.p1Cheat;
        bool p2Cheat = diceThrower.p2Cheat;
        bool rolling = diceThrower._isRolling;
        debugText.text = $"P1: {PlayerTurn.Instance.p1Score} | P2: {PlayerTurn.Instance.p2Score} (Current Player: {PlayerTurn.Instance.CurrentPlayerTurn}) \n Round Winner: {playerTurn.roundWinner} \n P1Money: {p1Money} | P2Money: {p2Money} \n P1MoneyDeduct: {PlayerTurn.Instance.p1MoneyDeduct} | P2MoneyDeduct: {PlayerTurn.Instance.p2MoneyDeduct} \n roundsPlayed: {PlayerTurn.Instance.roundsPlayed} \n _isRolling: {rolling} (diceThrower._finishedDiceCount: {diceThrower._finishedDiceCount}) \n P1Cheat: {p1Cheat} | P2Cheat: {p2Cheat}";
    }
}


/*
using UnityEngine;
using UnityEngine.UI;

public class DebugScipt : MonoBehaviour
{
    public Text debugText;
    private DiceThrowerScript diceThrower;
    private PlayerTurn playerTurn;

    void Start()
    {
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        playerTurn = FindFirstObjectByType<PlayerTurn>();
    }

    void Update()
    {
        // Basic null-safety to avoid NREs while objects initialize
        if (debugText == null)
            return;

        if (PlayerTurn.Instance == null || diceThrower == null || playerTurn == null)
        {
            debugText.text = "Waiting for PlayerTurn / DiceThrower to initialize...";
            return;
        }

        // Read money using the new read-only properties
        int p1Money = PlayerTurn.Instance.P1Money;
        int p2Money = PlayerTurn.Instance.P2Money;

        // DiceThrower debug values (still read directly)
        bool p1Cheat = diceThrower.p1Cheat;
        bool p2Cheat = diceThrower.p2Cheat;
        bool rolling = diceThrower._isRolling;

        // Build debug text
        debugText.text =
            $"P1: {PlayerTurn.Instance.p1Score} | P2: {PlayerTurn.Instance.p2Score} (Current Player: {PlayerTurn.Instance.CurrentPlayerTurn})\n" +
            $"Round Winner: {playerTurn.roundWinner}\n" +
            $"P1Money: {p1Money} | P2Money: {p2Money}\n" +
            $"P1MoneyDeduct: {PlayerTurn.Instance.p1MoneyDeduct} | P2MoneyDeduct: {PlayerTurn.Instance.p2MoneyDeduct}\n" +
            $"MoneyPool: {PlayerTurn.Instance.MoneyPool}\n" +
            $"roundsPlayed: {PlayerTurn.Instance.roundsPlayed}\n" +
            $"_isRolling: {rolling} (finishedDiceCount: {diceThrower._finishedDiceCount})\n" +
            $"P1Cheat: {p1Cheat} | P2Cheat: {p2Cheat}";
    }
}
*/