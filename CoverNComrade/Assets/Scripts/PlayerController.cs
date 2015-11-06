using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IInputReceiver))]
[RequireComponent(typeof(WeaponController))]
public class PlayerController : MonoBehaviour
{

    // PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
    public const int PlayerMaxHealth = 3;

    private Color _playerColor = Color.red;

    public Color PlayerColor
    {
        get { return _playerColor; }
        set
        {
            _playerColor = value;
            _mat.color = _playerColor;
        }
    }
    public float CursorStopDistance = 1;

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
    private Material _mat;

    public void Start()
    {
        // Components
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<IInputReceiver>();
        _weaponController = GetComponent<WeaponController>();
        _transf = transform;
        _mat = GetComponent<Renderer>().material;

        // Colorize
        _mat.color = PlayerColor;
    }

    public void FixedUpdate()
    {
        // Don't do anything if dead.
        if (Health <= 0)
            return;

        // Setup Raycast
        Ray ray = Camera.main.ScreenPointToRay(_input.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Move
            Vector3 dir = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(_transf.position.x, 0, _transf.position.z);
            if (dir.magnitude > CursorStopDistance)
                Move(dir);
            else
                _rb.velocity = Vector3.zero;
        }

    }

    public void Update()
    {
        // Don't do anything if dead.
        if (Health <= 0)
            return;

        if (_weaponController != null)
			ProcessShooting();
    }

    // Move moves the player to the given controller
    public void Move(Vector3 dir)
    {
        dir.Normalize();
        _rb.velocity = dir * Velocity * Time.fixedDeltaTime;
        _transf.forward = dir;
    }

    // ProcessShooting makes the player shoot to the current direction he's facing
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
    public void TakeDamage(int dmg, PlayerController enemy = null)
    {
        // Don't do anything if dead.
        if (Health <= 0)
            return;

        Health -= dmg;

		// TODO: sets the killer to the one who shot the bullet,
		//		 not needed right now as we don't need this in LMS mode.
		if (Health == 0) {
            if (GameManager.PlayerKilled != null)
    			GameManager.PlayerKilled(enemy, this);
            // Desaturate
		    StartCoroutine(FadeColor(2));
            // TODO: Disable team indicator
		}
    }

    // IsAlive returns whether or not the player is still alive.
    public bool IsAlive()
    {
        return Health > 0;
    }

    IEnumerator FadeColor(float t)
    {
        // Convert to HSB
        HSBColor newColor = new HSBColor(PlayerColor);
        float timeRemaining = t;
        while (timeRemaining > 0)
        {
            // Desaturate
            newColor.s = timeRemaining/t;
            PlayerColor = HSBColor.ToColor(newColor);
            // Decrease time
            timeRemaining -= Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        PlayerColor = Color.white;
    }
}
