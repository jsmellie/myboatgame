using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour
{
  #region Enums
  public enum Size
  {
    Small = 1,
    Medium = 2,
    Large = 3
  }
  #endregion

  #region Fields & Properties

  ShipController _shipController;

  [SerializeField]
  protected Transform _leftFiringPoint;
  [SerializeField]
  protected Transform _rightFiringPoint;

  [SerializeField]
  protected CanonBall _ball;
  public CanonBall ball
  {
    get
    {
      return _ball;
    }
  }

  [SerializeField]
  protected float _basePower = 1;
  public float basePower
  {
    get
    {
      return _basePower;
    }
  }

  [SerializeField]
  protected float _chargePower = 2;
  public float chargePower
  {
    get
    {
      return _chargePower;
    }
  }

  protected float _curChargeTime = 0;

  [SerializeField]
  protected float _chargeTime = 0.5f;
  public float chargeTime
  {
    get
    {
      return _chargeTime;
    }
  }

  [SerializeField]
  protected Size _size = Size.Medium;
  public Size size
  {
    get
    {
      return _size;
    }
  }

  [SerializeField]
  protected float _maxCooldown;
  public float maxCooldown
  {
    get
    {
      float ballModifier = 1.0f;

      if (ball != null)
      {
        ballModifier = ball.cooldownModifier;
      }
      return _maxCooldown * ballModifier;
    }
  }

  protected float _leftCooldown;
  public float leftCooldown
  {
    get
    {
      return _leftCooldown;
    }
  }

  protected float _rightCooldown;
  public float rightCooldown
  {
    get
    {
      return _rightCooldown;
    }
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

    if (controller == null)
    {
      Debug.LogError("No CanonController detected in hierarchy!");
      Destroy(this.gameObject);
      return;
    }

    controller.AddCanon(this);

    _shipController = controller.ship.shipController;

    //Set the size of the canon ball
    if (ball)
    {
      ball.SetSize(_size);
    }
  }

  protected virtual void Update()
  {
    //If a side is on cooldown, we decrement the amount
    if (_leftCooldown > 0)
    {
      _leftCooldown -= Time.deltaTime;

      if (_leftCooldown < 0)
      {
        _leftCooldown = 0;
      }
    }

    if (_rightCooldown > 0)
    {
      _rightCooldown -= Time.deltaTime;

      if (_rightCooldown < 0)
      {
        _rightCooldown = 0;
      }
    }
  }

  /// <summary>
  /// Fire the left canon.  If there's a charge amount, will fire with that, otherwise just uses base power
  /// </summary>
  /// <returns>Succeeded to fire or not</returns>
  public bool FireLeft()
  {
    if (_leftCooldown == 0)
    {
      _leftCooldown = maxCooldown;

      //TODO: Should probably pool the canon balls to make runtime memory management better but for now we'll just instantiate
      CanonBall newBall = Instantiate(ball) as CanonBall;

      Transform newBallXform = newBall.GetComponent<Transform>();
      newBallXform.position = _leftFiringPoint.position;
      newBallXform.rotation = _leftFiringPoint.rotation;

      Vector2 direction = this.GetComponent<Transform>().rotation * -Vector2.right;

      Rigidbody2D newBallBody = newBall.GetComponent<Rigidbody2D>();

      float chargePercent = _curChargeTime / chargeTime;

      float curPow = _basePower * (1 + (chargePercent * _chargePower));

      newBallBody.velocity = (direction * curPow);// +_shipController.curVelocity;
      newBall.Fired(chargePercent);

      return true;
    }

    return false;
  }


  /// <summary>
  /// Fire the Right canon.  If there's a charge amount, will fire with that, otherwise just uses base power
  /// </summary>
  /// <returns>Succeeded to fire or not</returns>
  public bool FireRight()
  {
    if (_rightCooldown == 0)
    {
      _rightCooldown = maxCooldown;

      CanonBall newBall = Instantiate(ball) as CanonBall;

      Transform newBallXform = newBall.GetComponent<Transform>();
      newBallXform.position = _rightFiringPoint.position;
      newBallXform.rotation = _rightFiringPoint.rotation;

      Vector2 direction = this.GetComponent<Transform>().rotation * Vector2.right;

      Rigidbody2D newBallBody = newBall.GetComponent<Rigidbody2D>();

      float chargePercent = _curChargeTime / chargeTime;

      float curPow = _basePower * (1 + (chargePercent * _chargePower));

      newBallBody.velocity = (direction * curPow);// +_shipController.curVelocity;
      newBall.Fired(chargePercent);

      return true;
    }

    return false;
  }
}
