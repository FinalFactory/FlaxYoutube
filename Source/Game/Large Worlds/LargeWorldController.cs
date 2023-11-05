using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// LargeWorldController Script.
/// </summary>
public class LargeWorldController : Script
{
    public Actor Camera;
    public UIControl CheckBox;
    public UIControl Button;
    public UIControl Distance;
    public UIControl CameraDistance;
    public UIControl LargeWorldOrigin;

    public override void OnStart()
    {
        base.OnStart();
        var checkBoxControl = ((CheckBox)CheckBox.Control);
        checkBoxControl.Checked = LargeWorlds.Enable;
        checkBoxControl.StateChanged += OnCheckBoxChanged;
        ((Button)Button.Control).Clicked += OnButtonClick;
        ((Slider)CameraDistance.Control).ValueChanged += OnCameraDistanceChanged;
    }

    private void OnCameraDistanceChanged()
    {
        //Due to a Issue: https://github.com/FlaxEngine/FlaxEngine/issues/1853 we need to use the default slider values.
        Camera.LocalPosition = new Vector3(0, 0, -(500 + ((Slider)CameraDistance.Control).Value * 100));
    }

    private void OnButtonClick()
    {
        var text = ((TextBox)Distance.Control).Text;
        if (double.TryParse(text, out double number))
        {
            Actor.Position = new Vector3(number,0, 0);   
        }
    }

    private void OnCheckBoxChanged(CheckBox obj)
    {
        LargeWorlds.Enable = obj.Checked;
        if (!LargeWorlds.Enable)
        {
            Vector3 x = default;
            LargeWorlds.UpdateOrigin(ref x, Vector3.Zero);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (LargeWorlds.Enable)
        {
            Vector3 x = Camera.Position;
            LargeWorlds.UpdateOrigin(ref x, Camera.Position);
            
            //Real chunkSizeInv = 1.0 / ChunkSize;
            //Real chunkSizeHalf = ChunkSize * 0.5;
            //origin = Vector3(Int3((position - chunkSizeHalf) * chunkSizeInv)) * ChunkSize;
            
            ((Label)LargeWorldOrigin.Control).Text = $" Chunk Size: {LargeWorlds.ChunkSize} \n Origin: {x} \n Distance to Origin: {(Camera.Position - x).Length:###0.0000}";
        }
    }
}