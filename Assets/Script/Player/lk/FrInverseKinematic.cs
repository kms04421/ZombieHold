using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    private Animator animator;
    private int layerIndex_Weapons;
    public bool enableIK = true; //  IK 켜기/끄기 제어용
    void Awake()
    {
        animator = GetComponent<Animator>();
        layerIndex_Weapons = animator.GetLayerIndex("Weapons");
    }
    /// <summary>
    /// IK함수 왼손 오른손 총기 설정위치에 고정
    /// </summary>
    /// <param name="_layerIndex"></param>

    private void OnAnimatorIK(int _layerIndex)
    {
        if (_layerIndex != layerIndex_Weapons || !enableIK)
        {
            return;
        }

        // 왼손 IK
        if (leftHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        }

        // 오른손 IK
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
    }
    /// <summary>
    /// IK활성화 비활성화
    /// </summary>
    /// <param name="active"></param>
    public void SetIKActive(bool active)
    {
        enableIK = active;
    }
}
