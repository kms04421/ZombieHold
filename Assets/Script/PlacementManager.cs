using UnityEngine;
using System.Collections;
public class PlacementManager : Singleton<PlacementManager>
{
    public GameObject prefab;        // 설치할 오브젝트
    private GameObject previewObj;   // 미리보기
    public float maxDistance = 5f;
    public bool isPlacing= false;
    public LayerMask placementMask;
    private Coroutine coroutine;
    public void StartPlacement(GameObject newPrefab, System.Action<bool> callback)
    {
        prefab = newPrefab;
        if (previewObj != null) Destroy(previewObj);
        previewObj = Instantiate(prefab);

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(PlacementRoutine(callback));
    }

    private IEnumerator PlacementRoutine(System.Action<bool> callback)
    {
        bool placementSuccess = false;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                previewObj.transform.position = SnapToGrid(hit.point);

                if (Input.GetMouseButtonDown(0) && CanPlace())
                {
                    Instantiate(prefab, previewObj.transform.position, Quaternion.identity);
                    placementSuccess = true;
                    break;
                }

                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    placementSuccess = false;
                    break;
                }
            }
            yield return null;
        }

        StopPlacement();
        callback?.Invoke(placementSuccess); // 성공 여부 전달
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x),
            Mathf.Round(pos.y),
            Mathf.Round(pos.z)
        );
    }

    private bool CanPlace()
    {
        // 여기에 충돌 검사, 자원 체크 등 넣기
        return true;
    }

    public void StopPlacement()
    {
        StopCoroutine(coroutine);
        isPlacing = false;
        if (previewObj != null) Destroy(previewObj);
    }
}