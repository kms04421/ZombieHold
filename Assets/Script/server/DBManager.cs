using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
public class DBManager : MonoBehaviour
{
    public List<ItemSO> itemSOs;
    void Start()
    {
        //  StartCoroutine(PostRequest());
    }
    public void DBItemsRequest(System.Action<List<Item>> onCompleted)
    {
        StartCoroutine(LoadItems(onCompleted));
    }
    IEnumerator LoadItems(System.Action<List<Item>> onCompleted)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/"))
        {
            yield return request.SendWebRequest();
            string json = request.downloadHandler.text;
            // Debug.Log("json 파싱" + json);
            string safeJson = EnsureJsonObject(json);

            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(safeJson);
            if (wrapper == null || wrapper.items == null)
            {
                Debug.LogError("파싱 실패");
                yield break;
            }

            List<Item> itemList = new List<Item>();
            foreach (var dto in wrapper.items)
            {
                ItemSO template = itemSOs.FirstOrDefault(x => x.id.Equals(dto.id));
                if (template == null)
                {
                    Debug.LogWarning($"ItemSO 없음: id={dto.id}");
                    continue;
                }

                Item newItem = new Item(template, dto.currentCount);
                //여기서 db수정할데이터 추가
                itemList.Add(newItem);
            }
            onCompleted?.Invoke(itemList); //  콜백으로 리스트 전달
        }
    }

    IEnumerator PostRequest()
    {
        string json = "{\"player\":\"Unity\",\"score\":100}";
        using (UnityWebRequest www = new UnityWebRequest("http://localhost:3000/data", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            Debug.Log(www.downloadHandler.text);
        }
    }
    private string EnsureJsonObject(string json)
    {
        string trimmed = json.TrimStart();
        if (trimmed.StartsWith("["))
            return "{\"items\":" + json + "}";
        return json;
    }

}