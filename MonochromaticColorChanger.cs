using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// This script should only run when loading the game or changing colors through the color changing buttons
/// 
/// </summary>

public class MonochromaticColorChanger : MonoBehaviour
{
    [Header("Color Changing")]
    [SerializeField] private List<Color> availableUIColors;
    [SerializeField] private List<Image> imagesToChangeColor;
    [SerializeField] private List<SpriteRenderer> spritesToChangeColor;
    [SerializeField] private int colorInt = 0;


    //This function goes onto 2 buttons
    //one of them will be the previous color button with isNext set to false
    //the other will be the next color button with isNext set to true

    public void NextColor(bool isNext)
    {
        UpdateColors(availableUIColors[isNext ? (colorInt++ >= availableUIColors.Count - 1) ? 0 : colorInt : (colorInt-- <= 0) ? availableUIColors.Count - 1 : colorInt]);
    }

    public void UpdateColors(Color _color)
    {
        foreach (Image _image in imagesToChangeColor)
        {
            _image.color = _color;
        }

        foreach (SpriteRenderer _sprite in spritesToChangeColor)
        {
            _sprite.color = _color;
        }
    }
}
