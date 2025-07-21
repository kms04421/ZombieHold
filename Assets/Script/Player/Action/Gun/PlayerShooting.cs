using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
public class PlayerShooting : MonoBehaviour
{
    private enum FireMode { SemiAuto, FullAuto }

    public Camera fpsCamera;        // 카메라 (총알 방향 기준)

    public float range = 100f;      // 총 사거리
    public float damage = 10f;      // 데미지
    const float flashDuration = 0.03f;  // muzzFlash유지시간

    public GameObject muzzleFlash;
    private Coroutine muzzleCoroutine;

    private FireMode fireMode = FireMode.FullAuto;

    public float fireRate = 0.1f; // 연발 속도
    private float nextFireTime = 0f;
    private bool isFiring = false;

    private AudioSource gunAudioSource;   // 총발사 소리용 AudioSource
    public AudioClip gunShotClip;        // 총발사 오디오 클립

    private void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (fireMode == FireMode.FullAuto && isFiring && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (fireMode == FireMode.SemiAuto)
        {
            if (context.performed) // 단발은 눌렀을 때 1번만
                Shoot();
        }
        else if (fireMode == FireMode.FullAuto)
        {
            if (context.started)
                isFiring = true;
            else if (context.canceled)
            {
                isFiring = false;
            }
                
        }
    }

    void Shoot()
    {
        Ray ray = fpsCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
          //  Debug.Log("맞췄다! 대상: " + hit.collider.name);

            // 예시: 맞은 대상이 Health 컴포넌트를 가지고 있다면 데미지 처리
            /*  var health = hit.collider.GetComponent<Health>();
              if (health != null)
              {
                  health.TakeDamage(damage);
              }*/

            // 이펙트나 사운드도 여기에 추가 가능
         
        }
        if (muzzleFlash != null)
        {
            if (muzzleFlash != null)
            {
                if (muzzleCoroutine != null)
                {
                    StopCoroutine(muzzleCoroutine);
                }
                muzzleCoroutine = StartCoroutine(FlashMuzzle());
            }
        }
        if (gunShotClip != null && gunAudioSource != null)
        {
            gunAudioSource.Stop();
            gunAudioSource.PlayOneShot(gunShotClip);
        }



        // 디버그 선
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);
    }
    IEnumerator FlashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        muzzleFlash.SetActive(false);
    }

}
