using UnityEngine;
using UnityEngine.SceneManagement;
using TiltFive;

public class backToMainMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check for Player One wand
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) ||
            UnityEngine.Input.GetButtonDown("Submit") ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.Two))
        {
            Destroy(GameObject.Find("BG Music"));
            SceneManager.LoadScene("MainMenu");
        }
    }
}
