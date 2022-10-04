using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Vector3 position, Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        
        position.z = camera.nearClipPlane;

        return camera.ScreenToWorldPoint(position);
    }
}
