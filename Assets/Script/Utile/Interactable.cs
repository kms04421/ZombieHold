using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public InputHandler inputHandler;


    public bool IsInteracting { get; private set; }
    public void StartInteract()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            OnStartInteract();
        }
    }

    public void StopInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            OnStopInteract();
        }
    }

    protected abstract void OnStartInteract();
    protected abstract void OnStopInteract();
}
