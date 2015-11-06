using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    public LayerMask BounceMask;
    public Color TrailColor;

    private float _speed = 10;
    private float _damage = 1;

    private float _lifeTime = 300;
    private float _skinWidth = 0.01f;
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

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", TrailColor);
    }

    void Update()
    {
        float moveDistance = _speed * Time.deltaTime;

        float distLeft = CheckBounceCollisions(moveDistance);
        if (distLeft > 0)
            CheckCollisions(distLeft);

        // transform.Translate(transform.forward * distLeft);
        transform.position += (transform.forward * distLeft);
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

    }

    float CheckBounceCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, moveDistance + _skinWidth, BounceMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
        }

        return moveDistance;

        /*
        // CHECK BOUNCE COLLISIONS
        float distanceLeft = moveDistance;
        int count = 0;
        while (distanceLeft > 0)
        {
            ++count;
            if (count > 20)
            {
                Debug.Log("Projectile had to many bounce checks.");
                return distanceLeft;
            }

            if (Physics.Raycast(ray, out hit, distanceLeft + _skinWidth, BounceMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 refelctDir = Vector3.Reflect(ray.direction, hit.normal);

                // check for player collisions before transforming
                CheckCollisions(hit.distance);

                Vector3 relocate = hit.point;
                relocate.y = _altitude;
                transform.position = relocate;

                Quaternion newRot = Quaternion.LookRotation(refelctDir);
                //newRot = Quaternion.Euler(new Vector3(0, newRot.eulerAngles.y, 0));
                transform.rotation = newRot;

                distanceLeft -= hit.distance;
            }
            else
            {
                return distanceLeft;
                //break;
            }

        }

        if (distanceLeft < 0)
            return 0;
        return distanceLeft;
         */
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        // Damage object HERE
    }
}
