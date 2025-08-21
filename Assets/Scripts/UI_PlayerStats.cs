using System.Collections;
using System.Collections.Generic;
using TiltFive;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UI_p1Stats : MonoBehaviour
{
    [SerializeField] private Text UiP1Stats;
    [SerializeField] private Text UiP2Stats;
    [SerializeField] private Text UiWinnerStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GUIStyle style = new GUIStyle ();
        // style.richText = true;
        // GUILayout.Label("<size=30>Some <color=yellow>RICH</color> text</size>",style);
    }

    // Update is called once per frame
    void Update()
    {
        UiP1Stats.text = "<color=lime>" + PlayerTurn.Instance.p1Money.ToString() + "$</color> " + PlayerTurn.Instance.p1Score;
        UiP2Stats.text = "<color=lime>" + PlayerTurn.Instance.p2Money.ToString() + "$</color> " + PlayerTurn.Instance.p2Score;

        if (PlayerTurn.Instance.roundsPlayed > 0)
        {
            if (PlayerTurn.Instance.gameWinner != 0)
            {
                UiWinnerStats.text = "<color=lime> Winner: " + PlayerTurn.Instance.gameWinner.ToString() + "</color>";
            }

            else if (PlayerTurn.Instance.roundWinner != 0)
            {
                UiWinnerStats.text = "<color=yellow> p" + PlayerTurn.Instance.roundWinner.ToString() + " won</color>";
            }
            else if (PlayerTurn.Instance.roundsPlayed == 0)
            {
                UiWinnerStats.text = "<color=yellow> Tie!</color>";
            }
        }
    }
}
