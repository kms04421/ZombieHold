using UnityEngine;

public class DeadState : IPlayerState
{
    private PlayerController player;

    public DeadState(PlayerController p) { player = p; }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.EnableMovement(false);
        // Á×À½ UI Ç¥½Ã µî
    }

    public void Exit() { }

    public void HandleInput() { }

    public void UpdateState() { }
}