// using Microsoft.Unity.VisualStudio.Editor;
using DG.Tweening;
using TiltFive;
// using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoundWinFlash : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image p1FlashImage;
    [SerializeField] private UnityEngine.UI.Image p2FlashImage;

    // private PlayerTurn playerTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartFlashEffect(int playerNumber)
    {
        StartCoroutine(FlashEffect(playerNumber));
    }
    private System.Collections.IEnumerator FlashEffect(int playerNumber)
    {
        if (playerNumber == 1)
        {
            // p1FlashImage.color = new Color(0, 1, 0, 0.5f);
            // p2FlashImage.color = new Color(1, 0, 0, 0.5f);
            p1FlashImage.DOColor(new Color(0, 1, 0, 0.5f), 0.2f);
            p2FlashImage.DOColor(new Color(1, 0, 0, 0.5f), 0.2f);
            yield return new WaitForSeconds(1f);
            p1FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);
            p2FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);
            // p1FlashImage.color = new Color(1, 1, 1, 0);
            // p2FlashImage.color = new Color(1, 1, 1, 0);
        }
        else if (playerNumber == 2)
        {
            p1FlashImage.DOColor(new Color(1, 0, 0, 0.5f), 0.2f);
            p2FlashImage.DOColor(new Color(0, 1, 0, 0.5f), 0.2f);
            yield return new WaitForSeconds(1f);
            p1FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);
            p2FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);

            // p1FlashImage.color = new Color(1, 0, 0, 0.5f);
            // p2FlashImage.color = new Color(0, 1, 0, 0.5f);
            // yield return new WaitForSeconds(1f);
            // p1FlashImage.color = new Color(1, 1, 1, 0);
            // p2FlashImage.color = new Color(1, 1, 1, 0);
        }
        else if (playerNumber == 0) // tie
        {
            p1FlashImage.DOColor(new Color(1, 1, 0, 0.5f), 0.2f);
            p2FlashImage.DOColor(new Color(1, 1, 0, 0.5f), 0.2f);
            yield return new WaitForSeconds(1f);
            p1FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);
            p2FlashImage.DOColor(new Color(1, 1, 1, 0), 0.5f);
        }
    }
}
