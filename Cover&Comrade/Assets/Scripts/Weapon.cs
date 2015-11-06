using UnityEngine;
using System.Collections;

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

	private float _nextShotTime;
	private bool _triggerReleasedSinceLastShot;
	private int _shotsRemainingInBurst;

	void Start () 
	{
		_shotsRemainingInBurst = BurstCount;
	}
	

	void Shoot () 
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
				Projectile newProjectile = Instantiate(MyProjectile, ProjectileSpawn[i].position, ProjectileSpawn[i].rotation) as Projectile;
				newProjectile.SetSpeed(MuzzleVelocity);
                newProjectile.SetDamage(DamagePerProjectile);
			}

			// Create bullet case ejection
			if (Shell != null)
			{
                Instantiate(Shell, ShellEjection.position, ShellEjection.rotation);
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
}
