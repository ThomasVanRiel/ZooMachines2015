using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController_Guntran))]
[RequireComponent(typeof(WeaponController))]
public class Player_Guntran : MonoBehaviour
{
    public float MoveSpeed = 5;

    private Camera _viewCamera = null;
    private PlayerController_Guntran _controller = null;
    private WeaponController _weaponController = null;

    void Start()
    {
        _controller = GetComponent<PlayerController_Guntran>();
        _viewCamera = Camera.main;
        _weaponController = GetComponent<WeaponController>();
    }

    void Update()
    {
        // Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = MoveSpeed * moveInput.normalized;
        _controller.Move(moveVelocity);

        // Look Input
        Ray ray = _viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            _controller.LookAt(point);
        }

        // Weapon Input
        if (Input.GetMouseButton(0))
        {
            _weaponController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _weaponController.OnTriggerRelease();
        }
    }
}
