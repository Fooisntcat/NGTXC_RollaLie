using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class gameWinner : MonoBehaviour
{
    [SerializeField] private Text P1Stats;
    [SerializeField] private Text P2Stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTurn.Instance.gameWinner == 1)
        {
            P1Stats.text = "<color=lime> Player 1 Wins!";
            P2Stats.text = "<color=red> Player 2 Loses!";
        }
        else if (PlayerTurn.Instance.gameWinner == 2)
        {
            P1Stats.text = "<color=red> Player 1 Loses!";
            P2Stats.text = "<color=lime> Player 2 Wins!";
        }
    }
}
