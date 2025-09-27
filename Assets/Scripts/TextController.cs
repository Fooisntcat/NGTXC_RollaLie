using System.Threading.Tasks;
using DG.Tweening;
using TiltFive;
using UnityEngine;
using UnityEngine.UI; // If you are using UI Text
using System.Collections;

// using TMPro; // Uncomment if using TextMeshPro

public class TextController : MonoBehaviour
{
    [SerializeField] private Text myText; // for Unity UI Text
    [SerializeField] private Image TextBackground;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform canvasRectTransform;
    // [SerializeField] private TextMeshProUGUI myText; // for TMP

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0, 0);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public void ShowText(string text, int playerID)
    {
        myText.gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.5f);
        if (playerID == 1)
        {
            canvasRectTransform.DORotate(new Vector3(90, 0, 0), 0.3f);
        }
        else if (playerID == 2)
        {
            canvasRectTransform.DORotate(new Vector3(90, 0, 180), 0.7f);
        }

        if (text == "flashbang")
        {
            myText.text = "<color=lime>5$</color> Flashbang: Use to temporarily blind your opponent.";
        }
        else if (text == "diceReroll")
        {
            myText.text = "<color=lime>10$</color> Dice Reroll: Reverts to the previous round.";
        }
        else if (text == "PumpNDump")
        {
            myText.text = "<color=lime>15$</color> PumpNDump: x2 Sum of Dice but if lose next round penalty x3.";
        }
        else if (text == "diceRerollActivated")
        {
            myText.text = "<color=lime>Dice Reroll Activated</color>";
            StartCoroutine(HideTextAfterDelay(2f));

        }
        else if (text == "PumpNDumpActivated")
        {
            myText.text = "<color=lime>PumpNDump Activated</color>";
            StartCoroutine(HideTextAfterDelay(2f));
            // myText.text = ""; // Clear the text after showing
        }
        else
        {
            Debug.Log("Hiding text");
            canvasGroup.DOFade(0, 0.5f);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            myText.text = "";
        }
    }

    public void HideText()
    {
        myText.gameObject.SetActive(false);
        myText.text = ""; // Clear the text
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasGroup.DOFade(0, 0.5f);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        myText.text = ""; // Clear the text after delay
    }
}
