using System.Collections.Generic;
using UnityEngine;

public class DebuffHandler : MonoBehaviour
{
    private List<Debuff> activeDebuffs = new List<Debuff>();

    public void ApplyDebuff(Debuff debuff)
    {
        if (!debuff.isStackable)
        {
            for (int i = 0; i < activeDebuffs.Count; i++)
            {
                if (activeDebuffs[i].type == debuff.type)
                {
                    activeDebuffs[i] = debuff; // 갱신
                    return;
                }
            }
        }
        activeDebuffs.Add(debuff);
    }

    // 현재 속도 감소 비율 계산
    public float GetSpeedModifier()
    {
        float speedModifier = 1f;

        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            Debuff debuff = activeDebuffs[i];
            if (debuff.type == DebuffType.Slow)
                speedModifier *= (1 - debuff.value);
        }

        return speedModifier;
    }

    // Stun 여부 확인
    public bool IsStunned()
    {
        foreach (var debuff in activeDebuffs)
        {
            if (debuff.type == DebuffType.Stun)
                return true;
        }
        return false;
    }

    // DOT, 체력 감소 등 필요 시 다른 Getter도 만들 수 있음
    public List<Debuff> GetActiveDebuffs() => activeDebuffs;
}