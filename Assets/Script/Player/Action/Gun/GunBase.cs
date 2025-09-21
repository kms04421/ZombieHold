using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
public class GunBase : MonoBehaviour
{
    [Header("총 데이터")]
    public GunData gunData;
    [Header("총구 위치")]
    [SerializeField]private Transform muzzPos;

    protected int currentAmmo;
    protected bool isFiring = false;
    protected Coroutine muzzleCoroutine;
    protected AudioSource audioSource;
    protected Transform cameraTransform;
    protected GameObject flash;


    private float nextFireTime = 0f;

    protected virtual void Awake()
    {
        currentAmmo = gunData.maxAmmo;
        audioSource = GetComponent<AudioSource>();
        cameraTransform = Camera.main.transform;
    }

 
    protected virtual void Update()
    {
        if (gunData.isFullAuto && isFiring && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + gunData.fireRate;
        }
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
        if (currentAmmo <= 0) return;

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

        currentAmmo--;
    }
    /// <summary>
    /// 총구 이펙트 효과
    /// </summary>
    /// <returns></returns>
    IEnumerator MuzzleFlashCoroutine()
    {
        if(flash == null)
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
    public void Reload()
    {
        currentAmmo = gunData.maxAmmo;
    }

}
