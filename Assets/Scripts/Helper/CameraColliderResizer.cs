using UnityEngine;
using System.Collections;

public class CameraColliderResizer : MonoBehaviour 
{
  void Awake()
  {
    BoxCollider2D collider = this.GetComponent<BoxCollider2D>();

    Camera camera = this.GetComponent<Camera>();

    if (collider2D != null && camera != null)
    {
      Vector2 camSize = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

      collider.size = camSize * 2;
      collider.center = Vector2.zero;
    }
  }
}
