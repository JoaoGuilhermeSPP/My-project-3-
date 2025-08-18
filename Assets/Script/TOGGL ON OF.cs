using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Image buttonImage; // Reference to the button's Image component
    public Image[] imagesToToggle; // Array of Images to show/hide
    public float lowOpacity = 0.3f; // Low opacity value
    private bool isOn = false; // Button state

    // Method to toggle the button and images
    public void Toggle()
    {
        isOn = !isOn; // Toggle the state

        if (isOn)
        {
            // If button is on, set full opacity and show images
            SetOpacity(1f);
            SetImagesVisibility(true);
        }
        else
        {
            // If button is off, set low opacity and hide images
            SetOpacity(lowOpacity);
            SetImagesVisibility(false);
        }
    }

    // Method to set the opacity of the button
    private void SetOpacity(float opacity)
    {
        Color color = buttonImage.color;
        color.a = opacity;
        buttonImage.color = color;
    }

    // Method to show/hide the series of images
    private void SetImagesVisibility(bool visible)
    {
        foreach (Image img in imagesToToggle)
        {
            img.enabled = visible;
        }
    }
}

