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
    public override void Shoot()
    {
        base.Shoot();
        StartRecoil();
    }


}
