using UnityEngine;
using System.Collections;

public class RecoilController : MonoBehaviour
{
    public Transform recoilTarget;     // 카메라나 총의 transform
    public float recoilAmount = 2f;    // 얼마나 튀는지
    public float recoilDuration = 0.1f;
    public float returnSpeed = 5f;

    private Quaternion originalRotation;
    private Coroutine recoilCoroutine;

    void Start()
    {
        originalRotation = recoilTarget.localRotation;
    }

    public void PlayRecoil()
    {
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(RecoilRoutine());
    }

    IEnumerator RecoilRoutine()
    {
        // 위로 튀는 반동
        Quaternion recoilRotation = originalRotation * Quaternion.Euler(-recoilAmount, 0f, 0f);
        recoilTarget.localRotation = recoilRotation;

        yield return new WaitForSeconds(recoilDuration);

        // 원래 위치로 되돌리기
        while (Quaternion.Angle(recoilTarget.localRotation, originalRotation) > 0.01f)
        {
            recoilTarget.localRotation = Quaternion.Slerp(recoilTarget.localRotation, originalRotation, Time.deltaTime * returnSpeed);
            yield return null;
        }

        recoilTarget.localRotation = originalRotation;
        recoilCoroutine = null;
    }
}