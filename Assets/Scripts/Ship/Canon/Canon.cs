using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

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
}
