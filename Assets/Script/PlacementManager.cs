using UnityEngine;
using System.Collections;
public class PlacementManager : Singleton<PlacementManager>
{
    public GameObject prefab;        // 설치할 오브젝트
    private GameObject previewObj;   // 미리보기
    public float maxDistance = 5f;
    public bool isPlacing= false;
    public LayerMask placementMask;
    public void StartPlacement(GameObject newPrefab)
    {
        prefab = newPrefab;
        if (previewObj != null) Destroy(previewObj);
        previewObj = Instantiate(prefab);

        StartCoroutine(PlacementRoutine());
    }

    private IEnumerator PlacementRoutine()
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                previewObj.transform.position = SnapToGrid(hit.point);

                if (Input.GetMouseButtonDown(0) && CanPlace())
                {
                    Instantiate(prefab, previewObj.transform.position, Quaternion.identity);
                    break; // 루프 종료 → 설치 완료
                }
            }
            yield return null; // 다음 프레임까지 대기
        }

        StopPlacement();
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
        isPlacing = false;
        if (previewObj != null) Destroy(previewObj);
    }
}