

//#define USE_RAYCAST

using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Interactor Script.
/// </summary>
public class Interactor : Script
{
    public float Distance = 100f;
    public float Radius = 10f;
    
    private IInteractable _lastInteractable;
    private InputEvent _interactionInputEvent;
#if FLAX_EDITOR
    private Vector3? _lastHitPoint;
#endif

    public event Action<IInteractable> EventLookAt; 

    public override void OnStart()
    {
        base.OnStart();
        _interactionInputEvent = new InputEvent("Interaction");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _interactionInputEvent.Pressed += OnInteractionPressed;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _interactionInputEvent.Pressed -= OnInteractionPressed;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _interactionInputEvent.Dispose();
    }

    private void OnInteractionPressed() => _lastInteractable?.OnInteraction();

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        
        IInteractable newInteractable = null;
#if FLAX_EDITOR
        _lastHitPoint = null;
#endif
#if USE_RAYCAST
        if (Physics.RayCastAll(Actor.Position, Actor.Direction, out var hits, Distance))
#else
        if (Physics.SphereCastAll(Actor.Position, Radius, Actor.Direction, out var hits, Distance))
#endif
        {
            foreach (var hit in hits)
            {
#if FLAX_EDITOR
               _lastHitPoint = hit.Point; 
#endif
                if (hit.Collider.IsActiveInHierarchy && hit.Collider.TryGetScript<IInteractable>(out var interactable))
                {
                    newInteractable = interactable;
                    break;
                }
            }
        }
            
        if (_lastInteractable != newInteractable)
        {
            _lastInteractable?.OnLookAway();
            newInteractable?.OnLookAt();
            _lastInteractable = newInteractable;
            EventLookAt?.Invoke(_lastInteractable);
        }
    }

#if FLAX_EDITOR
    public override void OnDebugDraw()
    {
        base.OnDebugDraw();
        if (FlaxEditor.Editor.IsPlayMode && _lastHitPoint.HasValue)
        {
            var boundingSphere = new BoundingSphere(_lastHitPoint.Value, Radius);
            DebugDraw.DrawWireSphere(boundingSphere, Color.Red);
        }
        else
        {
            var boundingSphere = new BoundingSphere(Actor.Position, Radius);
            var count = Distance / (Radius * 5);
            for (int i = 0; i <= count; i++)
            {
                boundingSphere.Center += Actor.Direction * Distance / count;
                DebugDraw.DrawWireSphere(boundingSphere, Color.Orange);
            }
        }
    }
#endif
}








