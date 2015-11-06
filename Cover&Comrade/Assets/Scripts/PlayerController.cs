using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IInputReceiver))]
public class PlayerController : MonoBehaviour {

	// PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
	public const int PlayerMaxHealth = 3;

	// Player's current health.
	private int _health;
	public int Health {
		get {
			return _health;
		}

		private set {
			_health = value;

			if (_health < 0) _health = 0;
			else if (_health > PlayerMaxHealth) _health = PlayerMaxHealth;
		}
	}

	public float Velocity = 1.0f;
	
	private Rigidbody _rb;

    private IInputReceiver _input;

	// TODO: Equip a WeaponController to the player controller
	// TODO: Input for the player controller
    // TODO: Subscribe to GameManager player die event

	public void Start()
    {
		_rb = GetComponent<Rigidbody>();
	    _input = GetComponent<IInputReceiver>();
	}
	
	public void FixedUpdate()
    {
		float moveX = _input.GetMouseX();
		float moveY = _input.GetMouseY();

		this.Move(new Vector3(moveX, 0, moveY));
	}

	public void Update()
    {
		if (_input.GetMouseButtonDown(0))
			Shoot();
	}

	// Move moves the player to the given controller
	public void Move(Vector3 dir)
    {
		// TODO: makes the player move
		_rb.velocity = dir * Velocity * Time.fixedDeltaTime;
	}

	// Shoot makes the player shoot to the current direction he's facing
	public void Shoot() {
		// TODO: makes the weapon controller shoots with its equipped weapon controller
		Debug.Log("SHOOTING");
	}

	// TakeDamage makes the player takes a given amount of damage
	public void TakeDamage(int dmg)
    {
		Health -= dmg;
	}

	// IsAlive returns whether or not the player is still alive.
	public bool IsAlive()
    {
		return Health > 0;
	}
}
