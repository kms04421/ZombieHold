using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffHandler : MonoBehaviour
{
    private List<Buff> activeDebuffs = new List<Buff>();
    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    /// <summary>
    /// 버프 추가,갱신
    /// </summary>
    /// <param name="debuff"></param>
    public void ApplyDebuff(Buff debuff)
    {
        if (!debuff.isStackable)
        {
            for (int i = 0; i < activeDebuffs.Count; i++)
            {
                if (activeDebuffs[i].type == debuff.type)
                {
                    activeDebuffs[i].duration = debuff.duration; // 남은 시간 갱신
                    activeDebuffs[i] = debuff; // 갱신
                    return;
                }
            }
        }
        activeDebuffs.Add(debuff);
        // 디버프 시간 관리 코루틴 시작
        StartCoroutine(DebuffDurationRoutine(debuff));
    }

    /// <summary>
    /// 개별 디버프의 지속시간을 감소시키는 코루틴
    /// </summary>
    private IEnumerator DebuffDurationRoutine(Buff debuff)
    {
        float remaining = debuff.duration;

        while (remaining > 0)
        {
            remaining -= Time.deltaTime;
            debuff.duration = remaining; // 남은 시간 갱신
            yield return delay;
        }

        // 시간 종료 시 제거
        activeDebuffs.Remove(debuff);
    }

    // 현재 속도 감소 비율 계산
    public float GetSpeedModifier()
    {
        float speedModifier = 1f;

        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            Buff debuff = activeDebuffs[i];
            if (debuff.type == BuffType.Slow)
                speedModifier *= (1 - debuff.value);
        }

        return speedModifier;
    }

    // Stun 여부 확인
    public bool IsStunned()
    {
        foreach (var debuff in activeDebuffs)
        {
            if (debuff.type == BuffType.Stun)
                return true;
        }
        return false;
    }

    // DOT, 체력 감소 등 필요 시 다른 Getter도 만들 수 있음
    public List<Buff> GetActiveDebuffs() => activeDebuffs;
}