using UnityEngine;
using YourGame.AI;
using System.Collections;
public class AttackBox : MonoBehaviour
{
    public Zombie Zombie;
    public bool isInAttackRange = false;
    public PlayerController player;
    private Coroutine waitExitCoroutine;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Zombie.DieChk()) return;
            isInAttackRange = true;
            player = other.GetComponent<PlayerController>();
            Zombie.ChangeState(Zombie.AttackState);
         
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInAttackRange = false;
            player = null;
            Zombie.Animator.SetBool("Attack", false);
            if (waitExitCoroutine != null)
                StopCoroutine(waitExitCoroutine); // 중복 방지

            waitExitCoroutine = StartCoroutine(WaitForAttackEndAndChangeState());
        }
    }
    /// <summary>
    /// 공격 애니메이션 종료까지 기다리고 상태 변경
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForAttackEndAndChangeState()
    {
        AnimatorStateInfo stateInfo = Zombie.Animator.GetCurrentAnimatorStateInfo(0);
   
        // "attack" 애니메이션이 끝날 때까지 대기
        while (!(stateInfo.IsName("attack") && stateInfo.normalizedTime >= 0.99f))
        {
            yield return null;
            stateInfo = Zombie.Animator.GetCurrentAnimatorStateInfo(0);
        }
        waitExitCoroutine = null;
        Zombie.ChangeState(Zombie.ChaseState);
    }
}
