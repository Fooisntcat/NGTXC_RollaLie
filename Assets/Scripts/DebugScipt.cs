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
        
        bool rolling = diceThrower._isRolling;
        debugText.text = $"P1: {PlayerTurn.Instance.p1Score} | P2: {PlayerTurn.Instance.p2Score} \n Current Player: {PlayerTurn.Instance.CurrentPlayerTurn} \n _isRolling: {rolling} \n Round Winner: {playerTurn.roundWinner} \n P1Money: {PlayerTurn.Instance.p1Money} \n P2Money: {PlayerTurn.Instance.p2Money} \n P1MoneyDeduct: {PlayerTurn.Instance.p1MoneyDeduct} \n P2MoneyDeduct: {PlayerTurn.Instance.p2MoneyDeduct} \n MoneyPool: {PlayerTurn.Instance.MoneyPool}";
    }
}
