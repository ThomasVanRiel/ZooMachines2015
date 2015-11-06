using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
	public const int PlayerMaxHealth = 3;

    public int MouseID;

	// Player's current health.
	public int health {
		get {
			return this.health;
		}

		private set {
			this.health = value;
			if (this.health < 0) this.health = 0;
			else if (this.health > PlayerMaxHealth) this.health = PlayerMaxHealth;
		}
	}

	public float velocity = 1.0f;
	
	private Rigidbody _rb;

	// TODO: Equip a WeaponController to the player controller
	// TODO: Input for the player controller

	public void Start() {
		this._rb = GetComponent<Rigidbody>();
	}
	
	public void FixedUpdate() {
		// TODO: makes the player move following the player's mouse cursor
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		this.Move(new Vector3(moveX, 0, moveY));
	}

	public void Update() {
		// TODO: gets the input from the player's mouse and makes the appropriate action
		if (Input.GetKey(KeyCode.Space))
			this.Shoot();
	}

	// Move moves the player to the given controller
	public void Move(Vector3 dir) {
		// TODO: makes the player move
		Debug.LogFormat("{0}", dir);
		this._rb.velocity = dir * this.velocity * Time.fixedDeltaTime;
	}

	// Shoot makes the player shoot to the current direction he's facing
	public void Shoot() {
		// TODO: makes the weapon controller shoots with its equipped weapon controller
		Debug.Log("SHOOTING");
	}

	// TakeDamage makes the player takes a given amount of damage
	public void TakeDamage(int dmg) {
		this.health -= dmg;
	}

	// IsAlive returns whether or not the player is still alive.
	public bool IsAlive() {
		return this.health > 0;
	}
}
