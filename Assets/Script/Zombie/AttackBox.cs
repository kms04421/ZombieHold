using UnityEngine;
using YourGame.AI;

public class AttackBox : MonoBehaviour
{
    public Zombie Zombie;
    private bool isInAttackRange = false;
    GameObject targetPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isInAttackRange = true;
            targetPlayer = other.gameObject;
            Zombie.ChangeState(Zombie.AttackState);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInAttackRange = false;
            targetPlayer = null;
        }
    }
    public void TryHit()
    {
        if (isInAttackRange && targetPlayer != null)
        {
            // 데미지 처리
           // targetPlayer.GetComponent<PlayerHealth>()?.TakeDamage(10);
        }
    }
}
