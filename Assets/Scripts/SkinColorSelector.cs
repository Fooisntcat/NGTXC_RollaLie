/* using UnityEngine;
using UnityEngine.UI;
using TiltFive;

public class SkinColorSelector : MonoBehaviour
{
    [Header("References")]
    public Renderer handRenderer;   // assign your hand mesh’s Renderer in the Inspector

    private Color chosenColor;

    // Call this on Button click
    public void RandomizeColor()
    {
        chosenColor = new Color(
            Random.value,   // R
            Random.value,   // G
            Random.value    // B
        );

        // Apply to the material
        handRenderer.material.color = chosenColor;
    }

    void SaveColor(Color c, int playerID)
    {
        PlayerPrefs.SetFloat("SkinR" + playerID, c.r);
        PlayerPrefs.SetFloat("SkinG" + playerID, c.g);
        PlayerPrefs.SetFloat("SkinB" + playerID, c.b);
        PlayerPrefs.Save();
    }

    // Load the color (use this in next scene)
    public static Color LoadColor(int playerID)
    {
        float r = PlayerPrefs.GetFloat("SkinR" + playerID, 1f);
        float g = PlayerPrefs.GetFloat("SkinG" + playerID, 1f);
        float b = PlayerPrefs.GetFloat("SkinB" + playerID, 1f);

        return new Color(r, g, b);
    }
    void Update()
    {
        // For testing: Press 'C' to randomize color
        if (UnityEngine.Input.GetKeyDown(KeyCode.C) || TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One))
        {
            RandomizeColor();
            SaveColor(chosenColor, 1);
        }
        if (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two))
        {
            RandomizeColor();
            SaveColor(chosenColor, 2);
        }
    }
}
*/
using UnityEngine;
using TiltFive;

public class SkinPalette : MonoBehaviour
{
    [Header("Preset Skin Colors")]
    public Color[] skinColors; // Assign palette in Inspector

    [Header("References")]
    public Renderer p1handRenderer; // Assign hand mesh for Player One
    public Renderer p2handRenderer; // Assign hand mesh for Player Two
    // public int playerID = 1; // 1 = Player One, 2 = Player Two

    private Color chosenColor;

    public void RandomizeSkin(int playerID)
    {
        int index = Random.Range(0, skinColors.Length);
        chosenColor = skinColors[index];

        ApplyColor(chosenColor, playerID);
        SaveColor(chosenColor, playerID);
    }

    public void ApplyColor(Color c, int playerID)
    {
        if (playerID == 1)
        {
            p1handRenderer.material.color = c;
        }
        else
        {
            p2handRenderer.material.color = c;
        }
    }

    public static void SaveColor(Color c, int playerID)
    {
        PlayerPrefs.SetFloat($"SkinR{playerID}", c.r);
        PlayerPrefs.SetFloat($"SkinG{playerID}", c.g);
        PlayerPrefs.SetFloat($"SkinB{playerID}", c.b);
        PlayerPrefs.Save();
    }

    public static Color LoadColor(int playerID)
    {
        float r = PlayerPrefs.GetFloat($"SkinR{playerID}", 1f);
        float g = PlayerPrefs.GetFloat($"SkinG{playerID}", 0.8f);
        float b = PlayerPrefs.GetFloat($"SkinB{playerID}", 0.6f);

        return new Color(r, g, b);
    }

    void Start()
    {
        // Load and apply saved color on start
        Color loadedColor = LoadColor(1);
        Color loadedColor2 = LoadColor(2);
        ApplyColor(loadedColor, 1);
        ApplyColor(loadedColor2, 2);
    }
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.A) || TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One))
        {
            RandomizeSkin(1);
        }
        if (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two))
        {
            RandomizeSkin(2);
        }
    }
}

