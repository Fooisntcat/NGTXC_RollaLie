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
        // Cheats
        bool? p1Cheat = diceThrower.GetLatestCheat(1);
        bool? p2Cheat = diceThrower.GetLatestCheat(2);
        bool rolling = diceThrower._isRolling;
        debugText.text = $"P1: {PlayerTurn.Instance.p1Score} | P2: {PlayerTurn.Instance.p2Score} (Current Player: {PlayerTurn.Instance.CurrentPlayerTurn}) \n Round Winner: {playerTurn.roundWinner} \n P1Money: {PlayerTurn.Instance.p1Money} | P2Money: {PlayerTurn.Instance.p2Money} \n P1MoneyDeduct: {PlayerTurn.Instance.p1MoneyDeduct} | P2MoneyDeduct: {PlayerTurn.Instance.p2MoneyDeduct} \n MoneyPool: {PlayerTurn.Instance.MoneyPool} \n roundsPlayed: {PlayerTurn.Instance.roundsPlayed} \n _isRolling: {rolling} (diceThrower._finishedDiceCount: {diceThrower._finishedDiceCount}) \nP1Cheat: {p1Cheat} | P2Cheat: {p2Cheat}";
    }
}
