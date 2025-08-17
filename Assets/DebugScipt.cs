using UnityEngine;
using UnityEngine.UI;

public class DebugScipt : MonoBehaviour
{
    public Text debugText;
    private DiceThrowerScript diceThrower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // diceThrowerScript = FindFirstObjectByType<DiceThrowerScript>();
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        bool rolling = diceThrower._isRolling;
        debugText.text = $"P1: {PlayerTurn.Instance.p1Score} | P2: {PlayerTurn.Instance.p2Score} \n Current Player: {PlayerTurn.Instance.CurrentPlayerTurn} \n _isRolling: {rolling}";
    }
}
