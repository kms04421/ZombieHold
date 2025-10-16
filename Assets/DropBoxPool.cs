using UnityEngine;

public class DropBoxPool : ObjectPoolBase<ItemDrop>
{
    [Header("inputHandler플레이어 상호작용용")]
    [SerializeField]private InputHandler inputHandler;

    private void Awake()
    {
        InitializePool();
        PoolManager.Instance.RegisterPool(this);
    }


    /// <summary>
    /// 풀을 미리 채워둠
    /// </summary>
    protected override void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            ItemDrop drop = Instantiate(prefab,transform);  // ObjectPoolBase의 prefab
            drop.inputHandler = inputHandler;
            drop.gameObject.SetActive(false);
            pool.Enqueue(drop); // pool은 protected로 열어두면 접근 가능
        }
    }
}
