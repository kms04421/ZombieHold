using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressable_Test : MonoBehaviour
{
    public AssetReference aref;

    public void Btn1()
    //변수로 가져와서 불러오기
    {
        Addressables.LoadAssetAsync<GameObject>(aref).Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                return;
            }

            Instantiate(op.Result, new Vector3(0, 1, 0), Quaternion.identity);
        };
    }

    public void Btn2()
    //직접 주소를 넣기
    {
        Addressables.LoadAssetAsync<GameObject>("Test/Black").Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded)
            {
                return;
            }

            Instantiate(op.Result, new Vector3(0, 1, 0), Quaternion.identity);

        };
    }
}
