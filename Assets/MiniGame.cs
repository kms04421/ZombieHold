using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public PlayerController player;

    private void OnEnable()
    {
        player.ChangeState(new MenuState(player));
    }

    private void OnDisable()
    {
        player.ChangeState(new NormalState(player));
    }
}
