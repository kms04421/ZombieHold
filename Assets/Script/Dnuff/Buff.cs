using UnityEngine;

public class Buff 
{
    public BuffType type;
    public float value;      // 효과 수치
    public float duration;   // 지속 시간
    public bool isStackable; // 스택 가능 여부

    public Buff(BuffType type, float value, float duration, bool isStackable = false)
    {
        this.type = type;
        this.value = value;
        this.duration = duration;
        this.isStackable = isStackable;
    }
}
public enum BuffType
{
    Slow,       // 이동 속도 감소
    Poison,     // 시간당 체력 감소
    Stun,       // 행동 불가
}