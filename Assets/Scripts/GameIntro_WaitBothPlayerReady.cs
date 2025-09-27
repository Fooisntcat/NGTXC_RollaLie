using UnityEngine;
using UnityEngine.SceneManagement;
using TiltFive;
using UnityEngine.UI;

public class TitFiveScript : MonoBehaviour
{
    private bool p1Ready = false;
    private bool p2Ready = false;
    [SerializeField] private Text p1Text;
    [SerializeField] private Text p2Text;

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) || UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            p1Ready = true;
            p2Ready = true;
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.One))
        {
            p1Ready = true;
            p1Text.text = "Waiting for other player...";
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.E) || TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.Two))
        {
            p2Ready = true;
            p2Text.text = "Waiting for other player...";
        }
        if (p1Ready && p2Ready)
        {
            p1Text.text = "Loading...";
            p2Text.text = "Loading...";
            SceneManager.LoadScene("RollaLie");
        }
    }
}
