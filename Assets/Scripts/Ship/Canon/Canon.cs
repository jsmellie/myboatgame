using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour
{
  #region Enums
  public enum DirectionType
  {
    Right = 0,
    Left,
    Forward,
    Backward,
    Follow
  }
  #endregion

  #region Fields & Properties

  [SerializeField]
  protected DirectionType _direction = DirectionType.Right;
  public DirectionType direction
  {
    get { return _direction; }
  }

  #endregion
  protected virtual void Awake()
  {
    CanonController controller;
    Transform obj = this.GetComponent<Transform>();

    do
    {
      controller = obj.GetComponentInParent<CanonController>();
      obj = obj.parent;
    } while (obj != null && controller == null);

    if(controller == null)
    {
      Debug.LogError("Non canon-controller detected in hierarchy.");
      Destroy(this.gameObject);
      return;
    }

    controller.AddCanon(this);
  }

  public bool Fire()
  {
    return true;
  }
}
