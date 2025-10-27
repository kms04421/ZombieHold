using UnityEngine;
using WebSocketSharp;


public class MultiClient : MonoBehaviour
{
    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("서버 연결 성공!");
            ws.Send("Hello Server from Unity!");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("서버 메시지 수신: " + e.Data);
        };

        ws.Connect();
    }

    void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
            ws.Close();
    }
}
