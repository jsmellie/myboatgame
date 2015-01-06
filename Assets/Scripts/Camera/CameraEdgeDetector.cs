using UnityEngine;
using System.Collections;

public class CameraEdgeDetector : MonoBehaviour 
{
  Vector2 _camMin;
  Vector2 _camMax;

  [SerializeField]
  bool showLogs = true;

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

      Log("Cam Min Pos: " + _camMin + " | Cam Max Pos: " + _camMax);
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    ShipController shipController = collider.GetComponent<ShipController>();

    //This is a ship!  If it's going offscreen we want to move it to the other side
    if (shipController != null)
    {
      //this.Log("Ship going off screen: " + shipController.name);

      Bounds colliderBounds = collider.bounds;

      this.Log("Ship bounds: " + colliderBounds + " | bounds size: " + colliderBounds.size);

      float halfShipSizeX = colliderBounds.size.x / 2;
      float halfShipSizeY = colliderBounds.size.y / 2;

      Vector3 position = collider.GetComponent<Transform>().position;

      //this.Log("Ship position: " + position);

      //Make sure the ship is within the camera bounds
      if (position.x < _camMin.x)
      {
        position.x = _camMax.x + halfShipSizeX;
      }
      else if (position.x > _camMax.x)
      {
        position.x = _camMin.x - halfShipSizeX;
      }

      if (position.y < _camMin.y)
      {
        position.y = _camMax.y + halfShipSizeY;
      }
      else if (position.y > _camMax.y)
      {
        position.y = _camMin.y - halfShipSizeY;
      }

      collider.GetComponent<Transform>().position = position;

      return;
    }

    CanonBall canonBall = collider.GetComponent<CanonBall>();

    //A canon ball is going off screen, delete it
    if (canonBall != null)
    {
      Destroy(collider.gameObject);

      return;
    }
  }

  protected void Log(string message)
  {
    if (showLogs)
    {
      Debug.Log(message);
    }
  }
}
