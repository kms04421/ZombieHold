using System.Collections;
using UnityEngine;
public enum GunType
{
    None,
    AR,
    SMG,
    ShotGun
}
public class GunBase : MonoBehaviour
{
    [Header("총 데이터")]
    public GunData gunData;
    [Header("총구 위치")]
    [SerializeField] private Transform muzzPos;

    private int _currentAmmo; //현재총알 
    public int CurrentAmmo // 현재 총알
    {
        get { return _currentAmmo; }
        set
        {
            _currentAmmo = value;
        }
    }

    //총기반동 스크립트
    public RecoilController recoilController;

    protected PlayerController playerController;
    protected bool isFiring = false;
    protected Coroutine muzzleCoroutine;
    protected AudioSource audioSource;
    protected Transform cameraTransform;
    protected GameObject flash;
    protected Inventory inventory;

    private float nextFireTime = 0f;

    [Header("탄창 용")]
    private Transform orgMagazineParnt;
    private Vector3 orgMagazinePos;
    private Quaternion orgMagazineRot;
    public Transform magazine;
    public Transform leftHandPos;

    protected virtual void Awake()
    {
        CurrentAmmo = gunData.maxAmmo;
        audioSource = GetComponent<AudioSource>();
        cameraTransform = Camera.main.transform;
    }

    protected virtual void Start()
    {
        inventory = SlotManager.Instance.inventory;
    }

    protected virtual void Update()
    {
        if (gunData.isFullAuto && isFiring && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + gunData.fireRate;
        }
    }
    public void SetPlayerController(PlayerController _playerController)
    {
        playerController = _playerController;
    }
    /// <summary>
    /// 총기 발사전 시작함수 (연발 단발 구분을 위해 사용)
    /// </summary>
    public virtual void StartFiring()
    {
        if (gunData.isFullAuto)
            isFiring = true;
        else
            Shoot();
    }
    /// <summary>
    /// 총기 발사 종료 함수
    /// </summary>
    public virtual void StopFiring()
    {
        isFiring = false;
    }


    /// <summary>
    /// 총기 발사
    /// </summary>
    public virtual void Shoot()
    {
        if (CurrentAmmo == 0) return;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, gunData.range))
        {
            var hitbox = hit.collider.GetComponent<HitBox>();
            if (hitbox != null)
                hitbox.OnHit(gunData.damage);
        }

        // muzzleFlash
        if (gunData.muzzleFlashPrefab != null)
        {
            if (muzzleCoroutine != null) StopCoroutine(muzzleCoroutine);
            muzzleCoroutine = StartCoroutine(MuzzleFlashCoroutine());
        }

        // 사운드
        if (audioSource != null && gunData.gunShotClip != null)
            audioSource.PlayOneShot(gunData.gunShotClip);

        CurrentAmmo--;
        PlayerUI.Instance.SetCurrentAmmo(CurrentAmmo);
    }
    /// <summary>
    /// 총구 이펙트 효과
    /// </summary>
    /// <returns></returns>
    IEnumerator MuzzleFlashCoroutine()
    {
        if (flash == null)
        {
            flash = Instantiate(gunData.muzzleFlashPrefab, muzzPos.position, muzzPos.rotation, transform);
        }

        flash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flash.SetActive(false);
    }
    /// <summary>
    /// 재장전
    /// </summary>
    public bool Reload()
    {
        string ammoID = GetGunAmmoId(gunData.type);

        int total = inventory.HasItemCount(ammoID);   // 인벤토리에 있는 총알 수
        if(total == 0 || CurrentAmmo == gunData.maxAmmo) return false;
        int needed = gunData.maxAmmo - CurrentAmmo;   // 탄창을 채우기 위해 필요한 총알 수

        if (total < needed)
        {
            // 인벤토리 총알이 부족 → 가진 만큼만 채움
            CurrentAmmo += total;
            inventory.RemoveItem(ammoID, total);
            total = 0;
        }
        else
        {
            // 인벤토리 총알이 충분 → 탄창 풀로 채움
            CurrentAmmo = gunData.maxAmmo;
            inventory.RemoveItem(ammoID, needed);
            total -= needed;
        }

        PlayerUI.Instance.SetCurrentAmmo(CurrentAmmo);
        PlayerUI.Instance.SetAllAmmo(total);
        return true;
    }

    public string GetGunAmmoId(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.None:
                break;

            case GunType.ShotGun:
                break;

            case GunType.AR:
                return "2_1";
                
            case GunType.SMG:
                break;
        }
        return "";
    }
    /// <summary>
    /// 총알 id반환
    /// </summary>
    /// <returns></returns>
    public string GetGunAmmoId()
    {
        return GetGunAmmoId(gunData.type);

    }

    public void Magazine()
    {
        orgMagazineParnt = magazine.parent;
        orgMagazinePos = magazine.localPosition;
        orgMagazineRot = magazine.localRotation;
        magazine.SetParent(leftHandPos);
        magazine.localPosition = Vector3.zero;
        magazine.localRotation = Quaternion.identity;
    }
    public void ReturnMagazine()
    {
        magazine.SetParent(orgMagazineParnt);
        magazine.localPosition = orgMagazinePos;
        magazine.localRotation = orgMagazineRot;
    }
}
