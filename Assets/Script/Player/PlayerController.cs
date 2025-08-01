using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayeData")]
    public PlayerData playerData;
    private void Awake()
    {
        playerData = new PlayerData();

    }
}
