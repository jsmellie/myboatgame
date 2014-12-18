using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
  #region Constants
  protected float[] DEFAULT_INCREMENTS = { 0.5f, 1.0f, 1.5f };
  #endregion

  #region Fields & Properties

  [SerializeField, Range(0.0001f, 1.0f)]
  protected float _movementAcceleration = 0.1f;

  [SerializeField, Range(0.0001f, 5.0f)]
  protected float _rotationAcceleration = 0.1f;
  [SerializeField]
  protected float _baseSpeed;
  [SerializeField]
  protected float[] _speedIncrements;

  protected int _speedIndex = -1;

  protected float _maxSpeed = float.NaN;
  protected float _currentSpeed = 0;

  protected Transform _xform;
  protected BoxCollider2D _boxCollider;

  protected Vector2 _camMin;
  protected Vector2 _camMax;
  #endregion
  #region Functions
  #region Unity Functions
  /// <summary>
  /// Called once 
  /// </summary>
  protected virtual void Awake()
  {
    //Save a ref to the xform to make it easier to use later
    _xform = this.GetComponent<Transform>();
    _boxCollider = this.GetComponent<BoxCollider2D>();
    Camera mainCam = Camera.main;

    if (mainCam != null)
    {
      Vector2 halfCamSize;
      halfCamSize.y = mainCam.orthographicSize;
      halfCamSize.x = halfCamSize.y * mainCam.aspect;

      Vector2 camPos = (Vector2)mainCam.transform.position;

      _camMin = camPos - halfCamSize;
      _camMax = camPos + halfCamSize;
    }

    //If the user hasn't set any speed increments, yell at them and set the defaults
    if (_speedIncrements.Length == 0)
    {
      Debug.LogError("You must have some speed increments for a Ship Controller. " + _xform.name);
      _speedIncrements = DEFAULT_INCREMENTS;
    }

    //If one of the speed increments is 1.0f, then use it as the default
    for (int i = 0; i < _speedIncrements.Length; ++i)
    {
      if (_speedIncrements[i] == 1.0f)
      {
        _speedIndex = i;
        break;
      }
    }

    //Else just use the middle
    if (_speedIndex < 0)
    {
      _speedIndex = Mathf.RoundToInt(_speedIncrements.Length / 2);
    }

    CalcMaxSpeed();
  }

  /// <summary>
  /// Called every frame to update the ship.
  /// </summary>
  protected virtual void Update()
  {
    //Make sure everything is set up nicely
    if (_maxSpeed != float.NaN)
    {
      //Calc new current speed
      CalcCurrentSpeed();

      //Calc the direction we are moving
      Vector2 direction = _xform.rotation * Vector2.up;

      Vector3 position = _xform.position;

      position += (Vector3)direction * _currentSpeed * Time.deltaTime;

      _xform.position = position;
    }

    if (_boxCollider != null)
    {
      float halfShipSize = _boxCollider.size.x > _boxCollider.size.y ? _boxCollider.size.x/2 : _boxCollider.size.y/2;

      Vector3 position = _xform.position;

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

      _xform.position = position;
    }
  }
  #endregion

  #region Speed Modifiers

  /// <summary>
  /// Increment the speed index.  If it's already the highest, won't do anything
  /// </summary>
  public virtual void SpeedUp()
  {
    _speedIndex += 1;

    CalcMaxSpeed();
  }

  /// <summary>
  /// Decreases the speed index.  If it's already the lowest, won't do anything
  /// </summary>
  public virtual void SlowDown()
  {
    _speedIndex -= 1;

    CalcMaxSpeed();
  }

  /// <summary>
  /// Calculates the current speed
  /// </summary>
  protected virtual void CalcCurrentSpeed()
  {
    if (_currentSpeed != _maxSpeed)
    {
      if (_currentSpeed > _maxSpeed)
      {
        _currentSpeed -= _movementAcceleration;
      }
      else
      {
        _currentSpeed += _movementAcceleration;
      }

      if (_currentSpeed > _maxSpeed - _movementAcceleration && _currentSpeed < _maxSpeed + _movementAcceleration)
      {
        _currentSpeed = _maxSpeed;
      }
    }
  }

  [ContextMenu("Calc Max Speed")]
  protected virtual void CalcMaxSpeed()
  {
    if (_speedIndex >= _speedIncrements.Length)
    {
      _speedIndex = _speedIncrements.Length - 1;
    }
    else if (_speedIndex < 0)
    {
      _speedIndex = 0;
    }

    _maxSpeed = _speedIncrements[_speedIndex] * _baseSpeed;
  }
  #endregion

  #region Direction Modifiers

  /// <summary>
  /// Rotates the ship based on the value passed
  /// </summary>
  /// <param name="intensity">The intensity to rotate, -1 being negative rotation and 1 being positive.  Clamped between -1 and 1.</param>
  public void Rotate(float intensity)
  {
    if (_currentSpeed > 0)
    {
      //Make sure that intensity is never over 1
      intensity = Mathf.Clamp(intensity, -1, 1);

      float rotAmount = _rotationAcceleration * intensity;

      _xform.Rotate(0, 0, rotAmount);
    }
  }

  #endregion

  #endregion
}
