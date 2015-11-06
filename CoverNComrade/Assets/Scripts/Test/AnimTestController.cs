using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class AnimTestController : MonoBehaviour {

    private CharacterController _characterController;
    private Animator _animator;
    private float _currentSpeed = 0.0f;

    public float MaxWalkSpeed = 10.0f;
    public float MaxRunSpeed = 20.0f;
    public float RotationSpeed = 120.0f;
    public float AccelerationSpeed = 5.0f;
    public bool IsRunning = false;

    //Call on creation
    void Awake() {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        //transform.Rotate(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update() {
        var verticalMovement = Input.GetAxis("Vertical");
        bool isVerticalPressed = (verticalMovement > 0) || (verticalMovement < 0);
        var horizontalMovement = Input.GetAxis("Horizontal");

        //ROTATIONS
        var rotationAmount = RotationSpeed * horizontalMovement * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);

        //MOVEMENT
        _currentSpeed += verticalMovement * AccelerationSpeed * Time.deltaTime;

        IsRunning = Input.GetKey(KeyCode.Y);
        var direction = Mathf.Sign(_currentSpeed);

        var maxSpeed = IsRunning ? MaxRunSpeed : MaxWalkSpeed;

        if (Mathf.Abs(_currentSpeed) > maxSpeed)
            _currentSpeed = maxSpeed * direction;

        //Deceleration
        if (!isVerticalPressed) {
            //Forward
            if (direction > 0) {
                _currentSpeed -= AccelerationSpeed * Time.deltaTime;
                if (_currentSpeed < 0)
                    _currentSpeed = 0;
            }
            //Backward
            else if (direction < 0) {
                _currentSpeed += AccelerationSpeed * Time.deltaTime;
                if (_currentSpeed > 0)
                    _currentSpeed = 0;
            }
        }

        //Displacement
        var displacement = transform.forward * _currentSpeed * Time.deltaTime;
        _characterController.Move(displacement);

        //ANIMATOR
        var normalizedSpeed = Mathf.Abs(_currentSpeed) / MaxRunSpeed;
        _animator.SetFloat("Speed", normalizedSpeed);
        _animator.SetBool("IsRunning", IsRunning);
        _animator.SetFloat("LeftRightDirection", horizontalMovement);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _animator.SetTrigger("Shoot");
        }
    }
}
