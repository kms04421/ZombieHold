using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerController player;

    public DeadState(PlayerController p) { player = p; }

    public void Enter()
    {
        Debug.Log("DeadState");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.animator.SetTrigger("Die");
        player.EnableMovement(false);
        // Á×À½ UI Ç¥½Ã µî
    }

    public void Exit() 
    {
        player.animator.SetTrigger("Revival");
        player.EnableMovement(true);
    }

    public void HandleInput() { }

    public void UpdateState() { }
}