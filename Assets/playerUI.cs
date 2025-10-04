using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : Singleton<playerUI>
{
    [SerializeField] private Image healthImg;
    [SerializeField] private TextMeshProUGUI currentAmmo;
    [SerializeField] private TextMeshProUGUI allAmmo;
    [SerializeField] private Image weaponImg;

    /// <summary>
    /// 현재 총알정보 세팅
    /// </summary>
    /// <param name="GunBase"> 총베이스 정보</param>
    public void SetCurrentAmmo(int value)
    {
        currentAmmo.text = value.ToString();
    }

    /// <summary>
    /// 전체 총알겟수 표시
    /// </summary>
    /// <param name="value"> 표시할 총알갯수</param>
    public void SetAllAmmo(int value)
    {
        allAmmo.text = value.ToString();
    }

    /// <summary>
    /// 무기 표시 이미지 변경
    /// </summary>
    /// <param name="image"></param>
    public void ChangeWeaponImp(Image image)
    {
        weaponImg = image;
    }

    /// <summary>
    /// 체력 비율(0~1)을 받아 fillAmount만 수정
    /// </summary>
    public void UpdateHealthUI(float ratio)
    {
        Debug.Log(ratio);
        ratio = Mathf.Clamp01(ratio);
        healthImg.fillAmount = ratio;
    }
}
