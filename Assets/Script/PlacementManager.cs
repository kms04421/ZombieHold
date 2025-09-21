using UnityEngine;
using System.Collections;
public class PlacementManager : Singleton<PlacementManager>
{
    [Header("설치할 오브젝트 프리펩")]
    public GameObject prefab;        // 설치할 오브젝트 보관용

    [Header("설치 환경 설정")]
    public float maxDistance = 5f;  // 설치 거리
    public LayerMask placementMask; // 설치할 레이어
    public bool isPlacing= false; // 설치 중인지 여부

    private GameObject previewObj;   // 미리보기

    private Coroutine coroutine; //코루틴 저장용

    /// <summary>
    /// 설치 시작 
    /// </summary>
    /// <param name="newPrefab"> 설치할 오브젝트</param>
    /// <param name="callback"> 설치가 완료 되었는지 확인하는 함수 변수로 보냄</param>
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

    /// <summary>
    /// 설치 코루틴 
    /// </summary>
    /// <param name="callback">설치가 완료 되었는지 확인하는 함수 변수로 보냄</param>
    /// <returns></returns>
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

    /// <summary>
    /// 소수점 위치를 정수로 변환
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 설치 취소
    /// </summary>
    public void StopPlacement()
    {
        StopCoroutine(coroutine);
        isPlacing = false;
        if (previewObj != null) Destroy(previewObj);
    }
}