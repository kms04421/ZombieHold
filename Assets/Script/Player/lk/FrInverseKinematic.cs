using UnityEngine;

public class FrInverseKinematic : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;

    public Transform test;
    public Transform testGun;
    private Vector3 orgPos;
    private Quaternion orgRot;
    public Transform orgParnt;

    private Animator animator;
    private int layerIndex_Weapons;

    [Header("IK 제어")]
    public bool enableIK = true;         // 전체 IK 제어
    public bool enableLeftHandIK = true; // 왼손만 개별 제어
    public bool enableRightHandIK = true; // 오른손만 개별 제어

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
            if (enableLeftHandIK)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            }
        }

        // 오른손 IK
        if (rightHand != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            if (enableRightHandIK)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
            }
        }

    }
    /// <summary>
    /// IK활성화 비활성화
    /// </summary>
    /// <param name="active"></param>
    public void SetIKReloadActive(bool active)
    {

        orgParnt = testGun.parent;
        if(!active)
        { 
            orgPos = testGun.localPosition;
            orgRot = testGun.localRotation;
            testGun.SetParent(test);
            // 원하면 로컬 위치/회전 초기화
            testGun.localPosition = Vector3.zero;
            testGun.localRotation = Quaternion.identity;
        }

            
        enableIK = active;
    }
    /// <summary>
    /// IK 비활성화
    /// </summary>
    /// <param name="active"></param>
    public void SetIK(bool active)
    {
        enableIK = active;
    }
    // 왼손 IK 개별 제어
    public void SetLeftHandIK(bool active)
    {
        enableLeftHandIK = active;
    }

    // 오른손 IK 개별 제어
    public void SetRightHandIK(bool active)
    {
        enableRightHandIK = active;
    }
    public void SetOrgPos()
    {
        testGun.SetParent(orgParnt);
        testGun.localPosition = orgPos;
        testGun.localRotation = orgRot;
    }
}
