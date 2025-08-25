using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    [Header("PlayeData")]
    public PlayerData playerData;
    [Header("GunData")]
    public GameObject weapon;
    public RecoilController weaponRecoil;
    private void Awake()
    {
        playerData = new PlayerData();
        animator = GetComponent<Animator>();
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
    public void Reload()
    {
        // IK 끄기
        GetComponent<FrInverseKinematic>().SetIKActive(false);

        // 리로드 애니메이션 실행
        animator.SetTrigger("Reload");

        // 코루틴으로 일정 시간 후 다시 IK 켜기 (애니메이션 길이에 맞춰 조절)
        StartCoroutine(RestoreIKAfterDelay(3.5f));
    }

    private IEnumerator RestoreIKAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<FrInverseKinematic>().SetIKActive(true);
    }
}
