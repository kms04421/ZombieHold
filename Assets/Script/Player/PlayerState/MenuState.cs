using UnityEditor;
using UnityEngine;

public class MenuState : IPlayerState
{
    private PlayerController player;

    public MenuState(PlayerController p) { player = p; }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.EnableMovement(false);
      //  MenuUI.Instance.Show();
    }

    public void Exit()
    {
     //   MenuUI.Instance.Hide();
        player.EnableMovement(true);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            player.ChangeState(player.NormalState);
    }

    public void UpdateState() { }
}