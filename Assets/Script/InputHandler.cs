using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
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

    public void OnSwitchSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int nextIndex = 1; // 마우스 휠 등
            SlotManager.Instance.SwitchSlot(nextIndex);
        }
    }
    public void Shoot(RecoilController weaponRecoil)
    {
        if (weaponRecoil != null)
        {
            weaponRecoil.PlayRecoil();
        }
    }
}