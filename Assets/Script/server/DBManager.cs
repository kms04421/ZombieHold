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
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/item"))
        {
            yield return request.SendWebRequest();
            string json = request.downloadHandler.text;
            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(json);
            if (wrapper == null || wrapper.item == null)
            {
                Debug.LogError("파싱 실패");
                yield break;
            }
            List<Item> itemList = new List<Item>();
            foreach (var dto in wrapper.item)
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
    public void DBZombiesRequest(System.Action<List<ZombieData>> onCompleted)
    {
        StartCoroutine(LoadZombie(onCompleted));
    }
    IEnumerator LoadZombie(System.Action<List<ZombieData>> onCompleted)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/zombie"))
        {
            yield return request.SendWebRequest();
            string json = request.downloadHandler.text;
            ZombieListWrapper wrapper = JsonUtility.FromJson<ZombieListWrapper>(json);
            if (wrapper == null || wrapper.zombie == null)
            {
                Debug.LogError("파싱 실패");
                yield break;
            }
            List<ZombieData> ZombieList = new List<ZombieData>();
            foreach (var dto in wrapper.zombie)
            {
                ZombieData newZombieData = new ZombieData(dto);
                //여기서 db수정할데이터 추가
                ZombieList.Add(newZombieData);
            }
            onCompleted?.Invoke(ZombieList); //  콜백으로 리스트 전달
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

}