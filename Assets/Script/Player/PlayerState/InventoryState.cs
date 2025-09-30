using UnityEngine;

public class InventoryState : IPlayerState
{
    private PlayerController player;

    public InventoryState(PlayerController p) { player = p; }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.EnableMovement(false);
        InventoryUI.Instance.Show();
    }

    public void Exit()
    {
        InventoryUI.Instance.Show();
        player.EnableMovement(true);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
            player.ChangeState(player.NormalState);
    }

    public void UpdateState() { }
}