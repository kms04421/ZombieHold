using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayeData")]
    public PlayerData playerData;
    [Header("GunData")]
    public GameObject weapon;
    public RecoilController weaponRecoil;
    private void Awake()
    {
        playerData = new PlayerData();

    }

    public void SetWeapon(GameObject go)
    {
        if(go != null)
        {
            weapon = go;
            RecoilController recoilController = go.GetComponent<RecoilController>();
            if(recoilController != null)
            {
                weaponRecoil = recoilController;
            }
            else
            {
                weaponRecoil = null;
            }
        }
    }

    public void Shoot()
    {
        if(weaponRecoil != null)
        {
            weaponRecoil.PlayRecoil();
        }
    }
}
