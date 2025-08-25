using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetRequest());
        StartCoroutine(PostRequest());
    }

    IEnumerator GetRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/"))
        {
            yield return www.SendWebRequest();
            Debug.Log(www.downloadHandler.text);
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