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
    private int _health = 1;
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

    // Components
    private Rigidbody _rb;
    private IInputReceiver _input;
    private WeaponController _weaponController;
    private Transform _transf;
    
    // TODO: Subscribe to GameManager player die event

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<IInputReceiver>();
        _weaponController = GetComponent<WeaponController>();
        _transf = transform;
    }

    public void FixedUpdate()
    { 
        // Setup Raycast
        Ray ray = Camera.main.ScreenPointToRay(_input.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Move
            Vector3 dir = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(_transf.position.x, 0, _transf.position.z);
            //Debug.Log(dir);
            Move(dir);
        }

    }

    public void Update()
    {
		if (_weaponController != null)
			ProcessShooting();
    }

    // Move moves the player to the given controller
    public void Move(Vector3 dir)
    {
        _rb.velocity = dir * Velocity * Time.fixedDeltaTime;
    }

    // ProcessShoting makes the player shoot to the current direction he's facing
    public void ProcessShooting()
    {
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
