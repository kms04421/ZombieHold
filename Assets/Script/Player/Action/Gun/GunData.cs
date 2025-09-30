using UnityEngine;
[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    [Header("총 세부정보")]
    public string gunName; //총 이름
    public float damage = 10f; //데미지
    public float fireRate = 0.1f;
    public float range = 100f; //거리
    public bool isFullAuto = false; //연발 가능여부
    public AudioClip gunShotClip; // 총발사 소리

    [Header("총알 관련")]
    public int maxAmmo = 30; 
    public GunType type;

    public GameObject muzzleFlashPrefab; // ? 이건 필요없는슷
}