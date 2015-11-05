using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
	public const int PlayerMaxHealth = 3;

	// Player's current health.
	public int Health {
		get {
			return this.Health;
		}

		private set {
			this.Health = value;
			if (this.Health < 0) this.Health = 0;
			else if (this.Health > PlayerMaxHealth) this.Health = PlayerMaxHealth;
		}
	}

	// TODO: Equip a WeaponController to the player controller
	// TODO: Input for the player controller
	
	public void FixedUpdate() {
		// TODO: makes the player move following the player's mouse cursor
	}

	public void Update() {
		// TODO: gets the input from the player's mouse and makes the appropriate action
	}

	// Move moves the player to the given controller
	public void Move(Vector3 dir) {
		// TODO: makes the player move
	}

	// Shoot makes the player shoot to the current direction he's facing
	public void Shoot() {
		// TODO: makes the weapon controller shoots with its equipped weapon controller
	}

	// TakeDamage makes the player takes a given amount of damage
	public void TakeDamage(int dmg) {
		this.Health -= dmg;
	}

	// IsAlive returns whether or not the player is still alive.
	public bool IsAlive() {
		return this.Health > 0;
	}
}
