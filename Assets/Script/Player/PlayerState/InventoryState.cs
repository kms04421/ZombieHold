using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class InventoryState : IPlayerState
{
    private PlayerController player;

    public InventoryState(PlayerController p) { player = p; }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InventoryUI.Instance.Show();
    }

    public void Exit()
    {
        InventoryUI.Instance.Show();
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
            player.ChangeState(player.NormalState);
    }

    public void UpdateState() 
    {
        player.ApplyGravity();
    }
}