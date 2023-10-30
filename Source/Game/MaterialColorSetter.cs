using FlaxEngine;

namespace Game;

public class MaterialColorSetter : Script
{
    public Material Material;

    public Color Color
    {
        get => (Color)Material.GetParameterValue("Color");
        set => Material.SetParameterValue("Color", value);
    }
}