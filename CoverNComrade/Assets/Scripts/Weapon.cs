using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public enum eFireMode { Single, Burst, Auto };
    public eFireMode FireMode = eFireMode.Single;

    public Transform[] ProjectileSpawn;
    public Projectile MyProjectile;
    public float TimeBetweenShots = 100;
    public float MuzzleVelocity = 35;
    public int DamagePerProjectile = 1;

    [Tooltip("The number of projectiles fired when in burst mode")]
    public int BurstCount = 3;	// number of projectiles fired when in burst mode

    public Transform Shell;
    public Transform ShellEjection;

    public int PoolSize = 10;
    private List<Projectile> _poolList = new List<Projectile>();
    private int _poolIndex = 0;
    private GameObject _poolHolder = null;


    private float _nextShotTime;
    private bool _triggerReleasedSinceLastShot = true;
    private int _shotsRemainingInBurst;
    private Color _trailColor = Color.white;

    void Start()
    {
        _shotsRemainingInBurst = BurstCount;

        _poolHolder = new GameObject(string.Format("{0} Bullet Pool", gameObject.name));
        InitializePool();
    }

    void OnDestroy()
    {
        Destroy(_poolHolder);
    }

    void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            if (FireMode == eFireMode.Burst)
            {
                if (_shotsRemainingInBurst <= 0)
                    return;
                --_shotsRemainingInBurst;
            }
            else if (FireMode == eFireMode.Single)
            {
                if (!_triggerReleasedSinceLastShot)
                    return;
            }

            // Instantiate bullet
            for (int i = 0; i < ProjectileSpawn.Length; ++i)
            {
                _nextShotTime = Time.time + TimeBetweenShots / 1000;
                PrepareProjectile(ProjectileSpawn[i].position, ProjectileSpawn[i].rotation);
                //Projectile newProjectile = Instantiate(MyProjectile, ProjectileSpawn[i].position, ProjectileSpawn[i].rotation) as Projectile;
                //newProjectile.SetSpeed(MuzzleVelocity);
                //newProjectile.SetDamage(DamagePerProjectile);
                //newProjectile.TrailColor = _trailColor;

                // Create bullet case ejection (for each bullet fired)
                if (Shell != null)
                {
                    Instantiate(Shell, ShellEjection.position, ShellEjection.rotation);
                }
            }
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
        _shotsRemainingInBurst = BurstCount;
    }

    public void SetTrailColor(Color newColor)
    {
        _trailColor = newColor;
    }

    public void InitializePool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            Projectile p = Instantiate(MyProjectile) as Projectile;
            p.transform.SetParent(_poolHolder.transform);
            _poolList.Add(p);
            p.gameObject.SetActive(false);
        }
    }

    public Projectile PrepareProjectile(Vector3 position, Quaternion rotation)
    {
        Projectile p = _poolList[_poolIndex++];
        _poolIndex = _poolIndex % PoolSize;

        p.transform.position = position;
        p.transform.rotation = rotation;
        p.SetDamage(DamagePerProjectile);
        p.SetSpeed(MuzzleVelocity);

        TrailRenderer tr = p.GetComponent<TrailRenderer>();
        p.TrailColor = _trailColor;
        float t = tr.time;
        tr.time = 0;
        tr.time = t;

        if (!p.gameObject.activeSelf)
            p.gameObject.SetActive(true);

        return p;
    }
}
