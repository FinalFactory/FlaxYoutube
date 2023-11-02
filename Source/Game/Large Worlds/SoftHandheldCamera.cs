#if USE_LARGE_WORLDS
using Real = System.Double;
#else
using Real = System.Single;
#endif

using FlaxEngine;

namespace Game;

public class CameraOscillationMovement : Script
{
    // The maximum height the camera will move up or down.
    [Tooltip("The maximum height difference for the camera movement.")]
    public float MaxHeightDifference = 0.5f;

    // The speed at which the camera moves up and down.
    [Tooltip("The speed at which the camera oscillates up and down.")]
    public float OscillationSpeed = 1.0f;

    // The maximum rotation angle in degrees.
    [Tooltip("The maximum rotation angle for the camera rotation.")]
    public float MaxRotationAngle = 5.0f;
    
    // The initial position of the camera.
    private Real _initialPosition;

    public override void OnStart()
    {
        // Store the initial position of the camera.
        _initialPosition = Actor.LocalPosition.Y;
    }

    public override void OnUpdate()
    {
        // Calculate the new Y position based on a sine wave.
        var newY = _initialPosition + Mathf.Sin(Time.GameTime * OscillationSpeed) * MaxHeightDifference;
        
        float rotationAngle = Mathf.Sin(Time.GameTime * OscillationSpeed + Mathf.PiOverTwo) * MaxRotationAngle;
        Quaternion newRotation = Quaternion.RotationAxis(Vector3.Up, Mathf.DegreesToRadians * (rotationAngle));
        // Update the camera's position.
        var pos = Actor.LocalPosition;
        pos.Y = newY;
        Actor.LocalPosition = pos;   
        Actor.Orientation = newRotation;
    }
}