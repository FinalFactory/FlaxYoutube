using System;
using FlaxEngine;

namespace Game;

/// <summary>
/// LidScript Script.
/// </summary>
public class LidScript : Script
{
    public float TargetAngle = 90.0f; // The angle to rotate to in degrees
    public float RotationSpeed = 30.0f; // Rotation speed in degrees per second

    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private float _t; // Time variable to control the rotation
    private float _direction = 0f; 

    public override void OnStart()
    {
        // Store the initial rotation
        _initialRotation = Actor.Orientation;

        // Calculate the target rotation based on the initial rotation and the target angle
        _targetRotation = _initialRotation * Quaternion.Euler(new Vector3(0, 0, TargetAngle));
    }

    public void Open()
    {
        _t = 0;
        _direction = 1f;
    }
    
    public void Close()
    {
        _direction = -1f;
    }
    
    public override void OnUpdate()
    {
        if (_t is >= 0f and <= 1.0f || _direction != 0f)
        {
            // Calculate the time step for rotation
            _t += Time.DeltaTime * (RotationSpeed / TargetAngle) * _direction;
            if (_t is < 0f or > 1.0f)
            {
                _direction = 0f;
                _t = Math.Clamp(_t, 0f, 1f); // Clamp the time variable to the range [0, 1]
            }
            Actor.Orientation = Quaternion.Lerp(_initialRotation, _targetRotation, _t);
        }
    }
}