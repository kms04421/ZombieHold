using System;
using UnityEngine;
public class GunAR : GunBase
{
    public void StartRecoil()
    {
        if (CurrentAmmo == 0) return;
        if (recoilController != null)
        {
            recoilController.PlayRecoil();
            playerController.ApplyRecoil(recoilController.xRecoilAmount,recoilController.yRandRecoil);
        }
            
    }
    public override void Shoot()
    {     
        base.Shoot();
        StartRecoil();
    }
}
