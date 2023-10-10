using FlaxEngine;

namespace Game;

public class Button : Script, IInteractable
{
    public ButtonPush ButtonPush;
    public LidScript LidScript;
    
    public void OnLookAt() => LidScript.Open();

    public void OnLookAway() => LidScript.Close();

    public void OnInteraction() => ButtonPush.Trigger();
}










