using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IInputReceiver))]
[RequireComponent(typeof(WeaponController))]
public class PlayerController : MonoBehaviour
{

    // PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
    public const int PlayerMaxHealth = 3;

    // Player's current health.
    private int _health;
    public int Health
    {
        get
        {
            return _health;
        }

        private set
        {
            _health = value;

            if (_health < 0) _health = 0;
            else if (_health > PlayerMaxHealth) _health = PlayerMaxHealth;
        }
    }

    public float Velocity = 1.0f;

    private Rigidbody _rb;

    private IInputReceiver _input;

    private WeaponController _weaponController;

    // TODO: Equip a WeaponController to the player controller
    // TODO: Input for the player controller
    // TODO: Subscribe to GameManager player die event

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<IInputReceiver>();
        _weaponController = GetComponent<WeaponController>();
    }

    public void FixedUpdate()
    {
        float moveX = _input.GetMouseX();
        float moveY = _input.GetMouseY();

        this.Move(new Vector3(moveX, 0, moveY));
    }

    public void Update()
    {
        ProcessShooting();
    }

    // Move moves the player to the given controller
    public void Move(Vector3 dir)
    {
        // TODO: makes the player move
        _rb.velocity = dir * Velocity * Time.fixedDeltaTime;
    }

    // ProcessShoting makes the player shoot to the current direction he's facing
    public void ProcessShooting()
    {
        //Debug.Log("SHOOTING");
        if (_input.GetMouseButton(0))
            _weaponController.OnTriggerHold();
        else if (_input.GetMouseButtonUp(0))
            _weaponController.OnTriggerRelease();
    }

    // SetWeapon gives the player a new weapon and removes the old one
    public void SetWeapon(Weapon newWeapon)
    {
        _weaponController.EquipWeapon(newWeapon);
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
