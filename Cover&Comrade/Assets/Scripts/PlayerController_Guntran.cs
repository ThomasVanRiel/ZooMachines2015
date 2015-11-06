using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController_Guntran : MonoBehaviour
{
    private Vector3 _velocity;
    private Rigidbody _rigidbody = null;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.deltaTime);
    }
}
