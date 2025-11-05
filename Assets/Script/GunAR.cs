public class GunAR : GunBase
{
    /// <summary>
    /// ÃÑ±â ¹Ýµ¿ ÃÑÈçµé¸®´Â È¿°ú
    /// </summary>
    public void StartRecoil()
    {
        if (CurrentAmmo == 0) return;
        if (recoilController != null)
        {
            recoilController.PlayRecoil();

        }

    }
    /// <summary>
    /// ÃÑ½î±â
    /// </summary>
    public override void Shoot(bool isLocalPlayer = true)
    {
        if (isLocalPlayer)
        {
            base.Shoot();
        }
        else
        {

            base.Shoot(false);
        }
    
        StartRecoil();
    }


}
