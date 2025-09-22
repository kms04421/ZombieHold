using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    public Interactable interactable;
    /// <summary>
    /// 슬롯 사용 (1~6)까지
    /// </summary>
    /// <param name="context"></param>
    public void OnUseSlot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 어떤 키인지 가져오기
        var keyControl = context.control as KeyControl;
        if (keyControl == null) return;

        int slotIndex = -1;

        switch (keyControl.keyCode)
        {
            case Key.Digit1: slotIndex = 0; break;
            case Key.Digit2: slotIndex = 1; break;
            case Key.Digit3: slotIndex = 2; break;
            case Key.Digit4: slotIndex = 3; break;
            case Key.Digit5: slotIndex = 4; break;
            case Key.Digit6: slotIndex = 5; break;          
        }

        if (slotIndex != -1)
        {
            SlotManager.Instance.UseUiSlot(slotIndex);
        }
    }
    /// <summary>
    /// 마우스 휠로 사용 슬롯 변경 
    /// </summary>
    /// <param name="context"></param>
    public void OnSwitchSlot(InputAction.CallbackContext context) //아직 미정
    {
        if (context.performed)
        {
            int nextIndex = 1; // 마우스 휠 등
         //   SlotManager.Instance.SwitchSlot(nextIndex);
        }
    }
    
    public void StartInteract()
    {
        if (interactable == null) return;
        interactable.StartInteract();
    }

    /// <summary>
    /// 총기 발사 아직 반동만있음
    /// </summary>
    /// <param name="weaponRecoil"></param>
    public void Shoot(RecoilController weaponRecoil)
    {
        if (weaponRecoil != null)
        {
            weaponRecoil.PlayRecoil();
        }
    }
}