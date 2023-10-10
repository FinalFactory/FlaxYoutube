using FlaxEngine;

namespace Game;

public class InteractableIndicator : Script
{
    public Interactor Interactor;
    public Actor Indicator;

    public override void OnEnable()
    {
        base.OnEnable();
        Interactor.EventLookAt += OnLookAt;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Interactor.EventLookAt -= OnLookAt;
    }

    private void OnLookAt(IInteractable obj) => Indicator.IsActive = obj != null;
}