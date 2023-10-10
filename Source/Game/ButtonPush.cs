using FlaxEngine;

namespace Game;

public class ButtonPush : Script
{
    public float TravelDistance = 1.0f; // The distance the object should travel down
    public float TravelSpeed = 1.0f; // The speed at which the object should travel

    private bool _triggerPush = false;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private float _t = -1f;

    public override void OnStart()
    {
        // Store the initial position
        _initialPosition = Actor.Position;

        // Calculate the target position based on the initial position and the travel distance
        _targetPosition = _initialPosition - new Vector3(0, TravelDistance, 0);
    }

    public void Trigger()
    {
        _triggerPush = true;
        if (_t < 0f)
        {
            _t = 0f;
        }
    }
    
    public override void OnUpdate()
    {
        if (_t < 0f)
        {
            return;
        }
        // Calculate the time step for movement
        float step = Time.DeltaTime * TravelSpeed;

        if (_triggerPush)
        {
            // Move to the target position
            Actor.Position = Vector3.Lerp(_initialPosition, _targetPosition, _t);

            // Increment time variable
            _t += step;

            // Clamp t to 1 to stay at target position
            if (_t >= 1.0f)
            {
                _t = 1.0f;
                _triggerPush = false; // Reset trigger
            }
        }
        else
        {
            // Move back to the initial position
            Actor.Position = Vector3.Lerp(_initialPosition, _targetPosition, _t);

            // Decrement time variable
            _t -= step;
        }
    }
}