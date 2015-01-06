using UnityEngine;
using System.Collections;

public class CameraColliderCreator : MonoBehaviour 
{
  [SerializeField]
  float _biggerColliderSizeDif = 1.0f;

#if UNITY_EDITOR
  [Header("Editor Variables"), SerializeField]
  bool renderColliderLines = true;

  [SerializeField]
  Color fullSizeColliderColor = Color.blue;
  [SerializeField]
  Color biggerColliderColor = Color.green;
#endif

  void Awake()
  {
    Camera camera = this.GetComponent<Camera>();

    if (camera != null)
    {
      Vector2 camSize = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

      BoxCollider2D fullSizeCollider = gameObject.AddComponent<BoxCollider2D>();
      fullSizeCollider.isTrigger = true;
      fullSizeCollider.size = camSize * 2;
      fullSizeCollider.center = Vector2.zero;

      BoxCollider2D biggerCollider = gameObject.AddComponent<BoxCollider2D>();
      biggerCollider.isTrigger = true;
      biggerCollider.center = fullSizeCollider.center;
      biggerCollider.size = fullSizeCollider.size + new Vector2(_biggerColliderSizeDif, _biggerColliderSizeDif);



#if UNITY_EDITOR
      if (renderColliderLines)
      {
        float camZ = camera.transform.position.z;

        Vector2 fullSizeColliderMin = fullSizeCollider.bounds.min;
        Vector2 fullSizeColliderMax = fullSizeCollider.bounds.max;
        Vector2 biggerColliderMin = biggerCollider.bounds.min;
        Vector2 biggerColliderMax = biggerCollider.bounds.max;

        //Rendering a line of the colliders so that you can see it without having to have it selected.
        Vector3 botLeft = new Vector3(fullSizeColliderMin.x, fullSizeColliderMin.y, camZ);
        Vector3 botRight = new Vector3(fullSizeColliderMax.x, fullSizeColliderMin.y, camZ);
        Vector3 topLeft = new Vector3(fullSizeColliderMin.x, fullSizeColliderMax.y, camZ);
        Vector3 topRight = new Vector3(fullSizeColliderMax.x, fullSizeColliderMax.y, camZ);

        Debug.DrawLine(botLeft, botRight, fullSizeColliderColor, float.PositiveInfinity);
        Debug.DrawLine(botRight, topRight, fullSizeColliderColor, float.PositiveInfinity);
        Debug.DrawLine(topRight, topLeft, fullSizeColliderColor, float.PositiveInfinity);
        Debug.DrawLine(topLeft, botLeft, fullSizeColliderColor, float.PositiveInfinity);

        botLeft = new Vector3(biggerColliderMin.x, biggerColliderMin.y, camZ);
        botRight = new Vector3(biggerColliderMax.x, biggerColliderMin.y, camZ);
        topLeft = new Vector3(biggerColliderMin.x, biggerColliderMax.y, camZ);
        topRight = new Vector3(biggerColliderMax.x, biggerColliderMax.y, camZ);

        Debug.DrawLine(botLeft, botRight, biggerColliderColor, float.PositiveInfinity);
        Debug.DrawLine(botRight, topRight, biggerColliderColor, float.PositiveInfinity);
        Debug.DrawLine(topRight, topLeft, biggerColliderColor, float.PositiveInfinity);
        Debug.DrawLine(topLeft, botLeft, biggerColliderColor, float.PositiveInfinity);
      }
#endif
    }
  }
}
