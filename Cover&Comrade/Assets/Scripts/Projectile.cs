using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    public LayerMask BounceMask;

    private float _speed = 10;
    private float _damage = 1;

    private float _lifeTime = 99999;
    private float _skinWidth = 0.05f;
    private float _altitude = 1.5f;

    void Start()
    {
        DestroyObject(gameObject, _lifeTime);

        _altitude = transform.position.y;

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, CollisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
    }

    void Update()
    {
        float moveDistance = _speed * Time.deltaTime;
        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // CHECK DAMAGE COLLISIONS
        if (Physics.Raycast(ray, out hit, moveDistance + _skinWidth, CollisionMask, QueryTriggerInteraction.UseGlobal))
        {
            OnHitObject(hit.collider, hit.point);
        }

        // CHECK BOUNCE COLLISIONS
        float distanceLeft = moveDistance;
        while (distanceLeft > 0)
        {
            if (Physics.Raycast(ray, out hit, distanceLeft + _skinWidth, BounceMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 refelctDir = Vector3.Reflect(transform.forward, hit.normal);

                Vector3 relocate = hit.point;
                relocate.y = _altitude;
                transform.position = relocate;

                Quaternion newRot = Quaternion.LookRotation(refelctDir);
                //newRot = Quaternion.Euler(new Vector3(0, 0, newRot.eulerAngles.z));
                transform.rotation = newRot;

                distanceLeft -= hit.distance;
            }
            else
            {
                distanceLeft = 0;
                break;
            }
        }
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        // Damage object HERE
    }
}
