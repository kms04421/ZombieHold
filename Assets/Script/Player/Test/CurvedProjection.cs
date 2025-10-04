using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CurvedProjection : MonoBehaviour
{
    private Camera cam;

    [Range(0f, 0.1f)]
    public float curveStrength = 0.02f; // °ªÀÌ Å¬¼ö·Ï ´õ ¸¹ÀÌ ±¸ºÎ·¯Áü

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Matrix4x4 p = cam.projectionMatrix;

        // È­¸é ÁÂ/¿ì(XÃà), »ó/ÇÏ(YÃà) °î·ü ¿Ö°î
        p[0, 2] = curveStrength;  // XÃà °î·ü
        p[1, 2] = curveStrength;  // YÃà °î·ü

        cam.projectionMatrix = p;
    }
}