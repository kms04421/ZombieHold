using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class NormalState : IPlayerState
{
    private PlayerController player;

    public NormalState(PlayerController p) { player = p; }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.EnableMovement(true);
    }

    public void Exit() { }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
            player.ChangeState(player.InventoryState);
        if (Input.GetKeyDown(KeyCode.Escape))
            player.ChangeState(player.MenuState);
    }

    public void UpdateState()
    {
        player.LookAround();
        player.Move();
    }
}