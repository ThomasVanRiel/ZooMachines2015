using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    public LayerMask BounceMask;
    public Color TrailColor = Color.white;
    public float ParticleHueOffset = .05f;
    private Color _particleColor = Color.white;

    public ParticleSystem TrailParticles;
    public GameObject BounceParticlesPrefab;
    public GameObject PlayerShotParticlesPrefab;

    private float _speed = 10;
    private int _damage = 1;

    private float _lifeTime = 300;
    private float _skinWidth = 0.01f;
    //private float _altitude = 1.5f;
    private TrailRenderer _tr = null;

    void Start()
    {
        //_altitude = transform.position.y;

        _tr = GetComponent<TrailRenderer>();

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, CollisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
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

    public void SetTrailColor(Color newColor)
    {
        TrailColor = newColor;
        if (_tr == null)
            _tr = GetComponent<TrailRenderer>();
        if (_tr != null)
            _tr.material.color = TrailColor;

        HSBColor partColor = new HSBColor(TrailColor);
        float hue = partColor.h;
        hue += ParticleHueOffset;
        if (hue > 1)
            hue -= 1;
        partColor.h = hue;
        _particleColor = HSBColor.ToColor(partColor);
        TrailParticles.startColor = _particleColor;
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

            // Add bounce particles
            Vector3 forw = Vector3.Cross(hit.normal, Vector3.up);
            Quaternion particlesRot = Quaternion.LookRotation(hit.normal, forw);
            if (Vector3.Cross(hit.normal, ray.direction).y > 0)
                particlesRot *= Quaternion.Euler(0, 0, 180);

            GameObject p = Instantiate(BounceParticlesPrefab, transform.position, particlesRot) as GameObject;
            p.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = _particleColor;
            Destroy(p, 2);
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
                GameObject p = Instantiate(PlayerShotParticlesPrefab, transform.position, transform.rotation) as GameObject;
                Destroy(p, 3);
                scr.TakeDamage(_damage);
            }
        }
    }

    public void ClearBulletTrail()
    {
        if (gameObject.activeSelf && _tr != null)
            StartCoroutine(ClearTrailRenderer());
    }

    IEnumerator ClearTrailRenderer()
    {
        var t = _tr.time;
        _tr.time = 0;
        yield return null;
        _tr.time = t;
        Debug.Log("Trail reset!");
    }
}
