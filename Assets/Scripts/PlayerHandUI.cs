using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PlayerHandUI : MonoBehaviour
{
    [SerializeField] private Text p1HandUI;
    [SerializeField] private Text p2HandUI;
    [SerializeField] private AudioSource moneySound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1HandUI.text = "";
        p2HandUI.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandUIUpdate(int player, int change)
    {
        moneySound.Play();
        if (player == 1)
        {
            if (change > 0)
            {
                p1HandUI.color = Color.green;
                p1HandUI.text = $"+{change.ToString()}$";
            }
            else if (change < 0)
            {
                p1HandUI.color = Color.red;
                p1HandUI.text = $"{change.ToString()}$";
            }
            else
            {
                p1HandUI.text = change.ToString();
            }

            p1HandUI.DOFade(1, 0.5f);
        }
        else if (player == 2)
        {
            if (change > 0)
            {
                p2HandUI.color = Color.green;
                p2HandUI.text = $"+{change.ToString()}$";
            }
            else if (change < 0)
            {
                p2HandUI.color = Color.red;
                p2HandUI.text = $"{change.ToString()}$";
            }
            else
            {
                p2HandUI.text = $"{change.ToString()}$";
            }

            p2HandUI.DOFade(1, 0.5f);
        }

        StartCoroutine(ClearHandUI(player));
    }

    private IEnumerator ClearHandUI(int player)
    {
        yield return new WaitForSeconds(2f);
        if (player == 1)
        {
            p1HandUI.DOFade(0, 0.5f);
        }
        else if (player == 2)
        {
            p2HandUI.DOFade(0, 0.5f);
        }
    }
}
