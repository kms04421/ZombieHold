using UnityEngine;
using System.Collections;

public class RecoilController : MonoBehaviour
{
    private Transform recoilTarget;
    public float xRecoilAmount = 2f;    //x값 한번 반동시 회전 정도
    public float yRecoilAmount = 2f;    //y값 한번 반동시 회전 정도
    public float yRandRecoil = 0f;
    public float recoilSpeed = 20f;    // 반동 올라가는 속도
    public float returnSpeed = 5f;     // 원래 위치로 돌아가는 속도

    private Quaternion originalRotation;
    private Coroutine recoilCoroutine;

    void Start()
    {
        recoilTarget = transform;
        originalRotation = recoilTarget.localRotation;
    }
    /// <summary>
    /// 총기 반동시작
    /// </summary>
    public void PlayRecoil()
    {
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(RecoilRoutine());
    }
    /// <summary>
    /// 총기 반동 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator RecoilRoutine()
    {
        // 좌우 반동 랜덤 값
        yRandRecoil = RandomRecoilValue();

        // 목표 회전: 현재 위치 기준으로 x, y 반동 적용
        Quaternion targetRotation = recoilTarget.localRotation * Quaternion.Euler(-xRecoilAmount, yRandRecoil, 0f);

        // 1) 반동 방향으로 이동
        while (Quaternion.Angle(recoilTarget.localRotation, targetRotation) > 0.01f)
        {
            recoilTarget.localRotation = Quaternion.Slerp(
                recoilTarget.localRotation,
                targetRotation,
                Time.deltaTime * recoilSpeed
            );
            yield return null;
        }

        // 2) 원래 위치로 돌아오기
        while (Quaternion.Angle(recoilTarget.localRotation, originalRotation) > 0.01f)
        {
            recoilTarget.localRotation = Quaternion.Slerp(
                recoilTarget.localRotation,
                originalRotation,
                Time.deltaTime * returnSpeed
            );
            yield return null;
        }

        recoilTarget.localRotation = originalRotation;
        recoilCoroutine = null;
    }

    /// <summary>
    /// y값 반동 랜덤 값
    /// </summary>
    /// <returns></returns>
    public float RandomRecoilValue()
    {
        return yRecoilAmount * Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
    }
}