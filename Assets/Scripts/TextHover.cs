using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you are using TextMeshPro

public class TextHover : MonoBehaviour
{
    public TMP_Text buttonText; // Reference to the TextMeshPro Text component
    private Color originalColor;

    // Define the hover color
    public Color hoverColor = Color.red;

    void Start()
    {
        // Get the original color of the text
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
    }

    // This function will be called when the mouse pointer enters the button area
    public void OnPointerEnter()
    {
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }

    // This function will be called when the mouse pointer exits the button area
    public void OnPointerExit()
    {
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }
}
