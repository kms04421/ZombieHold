using UnityEngine;
using SocketIOClient;
using System.Threading.Tasks;

public class MultiClient : MonoBehaviour
{
    private SocketIOUnity socket;
    public string roomName = "room1";

    async void Start()
    {
        socket = new SocketIOUnity("http://localhost:4000", new SocketIOOptions());

        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("멀티 서버 연결됨!");
            socket.Emit("joinRoom", roomName);
        };

        socket.On("playerJoined", (res) =>
        {
            string id = res.GetValue<string>();
            Debug.Log("다른 플레이어 입장: " + id);
        });

        socket.On("playerMove", (res) =>
        {
            var data = res.GetValue<PlayerData>();
            Debug.Log($"플레이어 이동 - {data.id}: ({data.x},{data.y},{data.z})");
            // TODO: 다른 플레이어 위치 갱신
        });

        socket.On("playerLeft", (res) =>
        {
            string id = res.GetValue<string>();
            Debug.Log("플레이어 퇴장: " + id);
        });

        await socket.ConnectAsync();
    }

    public void SendMove(Vector3 pos)
    {
        socket.Emit("playerMove", new PlayerData
        {
            room = roomName,
            id = socket.Id,
            x = pos.x,
            y = pos.y,
            z = pos.z
        });
    }
}
