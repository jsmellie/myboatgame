using UnityEngine;
using System.Collections;

public class CameraEdgeDetector : MonoBehaviour 
{
  Vector2 _camMin;
  Vector2 _camMax;

  void Awake()
  {
    Camera cam = this.GetComponent<Camera>();

    if (cam != null)
    {
      Vector2 halfCamSize;
      halfCamSize.y = cam.orthographicSize;
      halfCamSize.x = halfCamSize.y * cam.aspect;

      Vector2 camPos = (Vector2)cam.transform.position;

      _camMin = camPos - halfCamSize;
      _camMax = camPos + halfCamSize;
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    ShipController shipController = collider.GetComponent<ShipController>();

    //This is a ship!  If it's going offscreen we want to move it to the other side
    if (shipController != null)
    {
      Bounds colliderBounds = collider.bounds;

      float halfShipSize = colliderBounds.size.x > colliderBounds.size.y ? colliderBounds.size.x / 2 : colliderBounds.size.y / 2;

      Vector3 position = collider.GetComponent<Transform>().position;

      //Make sure the ship is within the camera bounds
      if (position.x < _camMin.x - halfShipSize)
      {
        position.x = _camMax.x + halfShipSize;
      }
      else if (position.x > _camMax.x + halfShipSize)
      {
        position.x = _camMin.x - halfShipSize;
      }

      if (position.y < _camMin.y - halfShipSize)
      {
        position.y = _camMax.y + halfShipSize;
      }
      else if (position.y > _camMax.y + halfShipSize)
      {
        position.y = _camMin.y - halfShipSize;
      }

      collider.GetComponent<Transform>().position = position;

      return;
    }

    CanonBall canonBall = collider.GetComponent<CanonBall>();

    //A canon ball is going off screen, delete it
    if (canonBall != null)
    {
      Destroy(collider.gameObject);
    }
  }
}
