using System.Collections;
using UnityEngine;
public class MultiPos : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SendPositionRoutine());
    }
    private IEnumerator SendPositionRoutine()
    {
        while (true)
        {

            ActorData data = new ActorData
            {
                id = MultiClient.Instance.myPlayerID,
                position = new PositionData
                {
                    x = transform.position.x,
                    y = transform.position.y,
                    z = transform.position.z
                },
                rotation = RotationData.FromQuaternion(transform.rotation)

            };
            NetworkMessage msg = new NetworkMessage
            {
                type = "playerUpdate",
                data = data
            };
            MultiClient.Instance.SendPlayerToSerber(msg);
            yield return new WaitForSeconds(0.05f); // 20fps Á¤µµ
        }
    }
}
