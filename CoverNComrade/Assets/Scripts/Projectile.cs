﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    public LayerMask BounceMask;
    public Color TrailColor = Color.white;

    private float _speed = 10;
    private int _damage = 1;

    private float _lifeTime = 300;
    private float _skinWidth = 0.01f;
    //private float _altitude = 1.5f;

    void Start()
    {
        DestroyObject(gameObject, _lifeTime);

        //_altitude = transform.position.y;

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, CollisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }

        GetComponent<TrailRenderer>().material.color = TrailColor; //.SetColor("_TintColor", TrailColor);
    }

    void Update()
    {
        float moveDistance = _speed * Time.deltaTime;

        CheckBounceCollisions(moveDistance);
        CheckCollisions(moveDistance);

        transform.position += (transform.forward * moveDistance);
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
    public void SetDamage(int newDamage)
    {
        _damage = newDamage;
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

    void CheckBounceCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + _skinWidth, BounceMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
        }
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        // Damage object HERE
        if (c != null)
        {
            var scr = c.gameObject.GetComponent<PlayerController>();
            if (scr != null)
            {
                scr.TakeDamage(_damage);
            }
        }
    }
}
