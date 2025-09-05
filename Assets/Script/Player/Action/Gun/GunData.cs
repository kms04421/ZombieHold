using UnityEngine;
[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;
    public float damage = 10f;
    public float fireRate = 0.1f;
    public float range = 100f;
    public bool isFullAuto = false;
    public int maxAmmo = 30;
    public GameObject muzzleFlashPrefab;
    public AudioClip gunShotClip;
}