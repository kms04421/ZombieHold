using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    private Animator animator;
    private int layerIndex_Weapons;

    void Awake()
    {
        animator = GetComponent<Animator>();
        layerIndex_Weapons = animator.GetLayerIndex("Weapons");
    }

    private void OnAnimatorIK(int _layerIndex)
    {
        if (_layerIndex != layerIndex_Weapons)
        {
            return;
        }

        // ¿Þ¼Õ IK
        if (leftHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        }

        // ¿À¸¥¼Õ IK
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
    }
}
