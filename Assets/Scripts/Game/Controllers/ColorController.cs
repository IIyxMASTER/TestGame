using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController
{
    private int[] _pickedColorsCount;

    
    private Color[] _colors = new[] {Color.red, Color.yellow, Color.green,};
    public Color Color { get; set; }

    public ColorController()
    {
        Color = Color.white;
    }

    public void PickColor(ColorEnum color)
    {
        int id = (int) color;
        Color = _colors[id];
        _pickedColorsCount[id]++;
    }
    public int GetColorPickCount(ColorEnum color)
    {
        return _pickedColorsCount[(int) color];
    }
}

public enum ColorEnum
{
    Red = 0,
    Yellow = 1,
    Green = 2
}