using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TiltFive;
using Unity.VisualScripting;
using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UI_PlayerStatsOnTable : MonoBehaviour
{
    [SerializeField] private Text UiP1Money;
    [SerializeField] private Text UiP2Money;
    [SerializeField] private Text UiP1Score;
    [SerializeField] private Text UiP2Score;
    [SerializeField] private Text UiWinnerStats;

    private bool isWinnerTextCleared = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GUIStyle style = new GUIStyle ();
        // style.richText = true;
        // GUILayout.Label("<size=30>Some <color=yellow>RICH</color> text</size>",style);
    }


    void Update()
    {
        UiP1Money.text = "<color=lime>" + PlayerTurn.Instance.P1Money.ToString() + "$</color> Roll: " + PlayerTurn.Instance.p1Score.ToString();
        UiP2Money.text = "<color=lime>" + PlayerTurn.Instance.P2Money.ToString() + "$</color> Roll: " + PlayerTurn.Instance.p2Score.ToString();
        // UiP1Score.text = "P1: " + PlayerTurn.Instance.p1Score.ToString();
        // UiP2Score.text = "P2: " + PlayerTurn.Instance.p2Score.ToString();
        
        // if (PlayerTurn.Instance.CurrentPlayerTurn == 2 && PlayerTurn.Instance.roundsPlayed == 0)
        // {
        //     UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
        // }


        /*
        if (PlayerTurn.Instance.roundsPlayed > 0)
        {
            if (PlayerTurn.Instance.gameWinner != 0)
            {
                UiWinnerStats.text = "<color=lime> Winner: " + PlayerTurn.Instance.gameWinner.ToString() + "</color>";
            }

            else if (PlayerTurn.Instance.roundWinner != 0)
            {
                UiWinnerStats.text = "<color=yellow> p" + PlayerTurn.Instance.roundWinner.ToString() + " won</color>";
                // UiWinnerStats.text += " and earned <color=lime>" + PlayerTurn.Instance.MoneyPool.ToString() + "$</color>";
                if (PlayerTurn.Instance.roundWinner == 1)
                {
                    UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * smooth);
                    StartCoroutine(ClearWinnerText());

                }
                else if (PlayerTurn.Instance.roundWinner == 2)
                {
                    UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(90, 0, 180), Time.deltaTime * smooth);
                    StartCoroutine(ClearWinnerText());
                }
                else if (PlayerTurn.Instance.roundWinner == 0)
                {
                    UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * smooth);
                    StartCoroutine(ClearWinnerText());
                }
            }
            else if (PlayerTurn.Instance.roundWinner == 0)
            {
                UiWinnerStats.text = "<color=yellow> Tie!</color>";
                StartCoroutine(ClearWinnerText());
            }
        }
        else
        {
            // UiWinnerStats.text = " ";
        }
        */

        /*
        if (isWinnerTextCleared == true)
        {
            if (PlayerTurn.Instance.CurrentPlayerTurn == 1)
            {
                UiWinnerStats.text = "It's your turn, p1!";
                UiWinnerStats.DOFade(1, 0.5f);
                UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            }
            else if (PlayerTurn.Instance.CurrentPlayerTurn == 2)
            {
                UiWinnerStats.text = "It's your turn, p2!";
                UiWinnerStats.DOFade(1, 0.5f);
                // UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
            }
        }
        */
    }

    public void roundFinished(int roundWinner, int MoneyPool)
    {
        
        if (UiWinnerStats == null) return;
        Debug.Log("roundFinished called with roundWinner: " + roundWinner + " and MoneyPool: " + MoneyPool);


        if (roundWinner == 1)
        {
            UiWinnerStats.text = "<color=yellow> p" + roundWinner.ToString() + " won</color> and earned <color=lime>" + MoneyPool.ToString() + "$</color>";
            // UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * smooth);
            UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            StartCoroutine(ClearWinnerText());
        }
        else if (roundWinner == 2)
        {
            UiWinnerStats.text = "<color=yellow> p" + roundWinner.ToString() + " won</color> and earned <color=lime>" + MoneyPool.ToString() + "$</color>";
            // UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * smooth);
            UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
            StartCoroutine(ClearWinnerText());
        }
        else if (roundWinner == 0)
        {
            UiWinnerStats.text = "<color=yellow> Tie!</color> Each earned <color=lime>" + MoneyPool.ToString() + "$</color>";
            // UiWinnerStats.transform.rotation = Quaternion.Slerp(UiWinnerStats.transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * smooth);
            UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            StartCoroutine(ClearWinnerText());
        }
        else
        {
            // UiWinnerStats.text = " ";
        }
    }

    IEnumerator ClearWinnerText()
    {
        isWinnerTextCleared = false;
        yield return new WaitForSeconds(1.5f);
        UiWinnerStats.DOFade(0, 0.5f);
        isWinnerTextCleared = true;
        yield return new WaitForSeconds(0.25f);
        
        if (PlayerTurn.Instance.CurrentPlayerTurn == 1)
        {
            UiWinnerStats.text = "It's your turn, p1!";
            UiWinnerStats.DOFade(1, 0.5f);
            UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
        }
        else if (PlayerTurn.Instance.CurrentPlayerTurn == 2)
        {
            UiWinnerStats.text = "It's your turn, p2!";
            UiWinnerStats.DOFade(1, 0.5f);
            UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
        }
    }


    public void PlayerTurnFinished(int currentPlayerTurn)
    {
        // Debug.Log("PlayerTurnFinished called with currentPlayerTurn: " + currentPlayerTurn);
        StartCoroutine(WaitFinishClearWinnerText());
        // Debug.Log("isWinnerTextCleared: " + isWinnerTextCleared);

        if (PlayerTurn.Instance.roundsPlayed < 1)
        {
            if (currentPlayerTurn == 1)
            {
                UiWinnerStats.text = "Press 1 to roll dice, p1!";
                UiWinnerStats.DOFade(1, 0.5f);
                UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            }
            else if (currentPlayerTurn == 2)
            {
                UiWinnerStats.text = "Press 1 to roll dice, p2!";
                UiWinnerStats.DOFade(1, 0.5f);
                UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
            }
        }
        else
        {
            if (currentPlayerTurn == 1)
            {
                UiWinnerStats.text = "It's your turn, p1!";
                UiWinnerStats.DOFade(1, 0.5f);
                UiWinnerStats.transform.DORotate(new Vector3(90, 0, 0), 0.5f);
            }
            else if (currentPlayerTurn == 2)
            {
                UiWinnerStats.text = "It's your turn, p2!";
                UiWinnerStats.DOFade(1, 0.5f);
                UiWinnerStats.transform.DORotate(new Vector3(90, 0, 180), 0.5f);
            }
        }

        // UiWinnerStats.DOFade(1, 0.5f);

        // Debug.Log("PlayerTurnFinished called with currentPlayerTurn: " + currentPlayerTurn);


        
    }

    IEnumerator WaitFinishClearWinnerText()
    {
        while (!isWinnerTextCleared)
        {
            yield return null;
        }
    }
}