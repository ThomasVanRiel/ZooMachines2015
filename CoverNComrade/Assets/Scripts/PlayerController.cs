using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IInputReceiver))]
[RequireComponent(typeof(WeaponController))]
public class PlayerController : MonoBehaviour
{

    // PlayerMaxHealth is the player's maximum amount of health the player can have at any given time.
    public const int PlayerMaxHealth = 3;

    private bool _isRunning = false;
    public bool CanMove = false;
    public bool CanShoot = false;

    private Plane _plane = new Plane(Vector3.up, Vector3.zero);

    private Vector3 _prevForward;
    private float _prevLeftRightDirection = 0;
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

            Vector4 shapeScales = Vector4.zero;
            float scaleSteps = .3f;
            if (_health > 0)
            {
                shapeScales.x = 1;
                if (_health > 1)
                {
                    shapeScales.y = 1 + scaleSteps;
                    if (_health > 2)
                        shapeScales.z = 1 + 2 * scaleSteps;
                }
            }
            PlayerIndicator.material.SetVector("Shape Scale", shapeScales);
        }
    }

    public float Velocity = 1.0f;

    // Components
    private Rigidbody _rb;
    private IInputReceiver _input;
    private WeaponController _weaponController;
    private Transform _transf;
    //public Renderer MeshRender;
    //private Material _mat;
    private Animator _animator;
    private TeamController _teamC;
    public Projector PlayerIndicator;
    public RagdollScript RagDoll;

    public void Start()
    {
        // Components
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<IInputReceiver>();
        _weaponController = GetComponent<WeaponController>();
        _transf = transform;
        //_mat = MeshRender.material;
        _animator = GetComponentInChildren<Animator>();
        _teamC = GetComponent<TeamController>();

        // Colorize
        //_mat.color = PlayerColor;

        // 
        _prevForward = _transf.forward;

    }

    public void FixedUpdate()
    {
        // Don't do anything if dead.
        if (Health <= 0)
            return;

        ProcessMovement();

    }

    public void Update()
    {
        ProcessAnimations();

        // Don't do anything if dead.
        if (Health <= 0)
            return;

        if (_weaponController != null && CanShoot)
            ProcessShooting();
    }

    // Move moves the player to the given controller
    public void Move(Vector3 dir)
    {
        dir.Normalize();
        Vector3 vel = dir * Velocity * Time.fixedDeltaTime;
        vel.y = _rb.velocity.y;
        _rb.velocity = vel;
        _transf.forward = dir;
    }

    void ProcessMovement()
    {
        if (!CanMove)
            return;

        //// TEST FOR JOYSTICKS        
        //Vector3 dirr = Vector3.zero;
        //int pi = GetComponent<MouseInputReceiver>().PlayerID;
        //dirr.x = Input.GetAxis(string.Format("p{0}_MoveX", pi+1));
        //dirr.y = Input.GetAxis(string.Format("p{0}_MoveY", pi+1));
        //dirr.Normalize();
        //if (dirr.magnitude > .1f)
        //{
        //    Move(dirr);
        //    _isRunning = true;
        //}
        //else
        //{
        //    _isRunning = false;
        //    _rb.velocity = Vector3.zero;
        //}
        //return;

        // Setup Raycast
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        //Debug.Log(playerPos + " " + _input.GetMousePosition());
        Ray ray = Camera.main.ScreenPointToRay(_input.GetMousePosition() + playerScreenPos);
        float hit;
        if (_plane.Raycast(ray, out hit))
        {
            Vector3 point = ray.GetPoint(hit);
            //Debug.Log(point - new Vector3(_transf.position.x, 0, _transf.position.z));
            // Move
            Vector3 dir = point - new Vector3(_transf.position.x, 0, _transf.position.z);
            if (dir.magnitude > CursorStopDistance)
            {
                Vector3 newPlayerScreenPos = Camera.main.WorldToScreenPoint(transform.position + (_rb.velocity * Time.fixedDeltaTime));
                _input.OffsetMousePosition(-(new Vector2(newPlayerScreenPos.x, newPlayerScreenPos.y) - new Vector2(playerScreenPos.x, playerScreenPos.y)));
                Move(dir);
                _isRunning = true;
            }
            else
            {
                _rb.velocity = Vector3.zero;
                _isRunning = false;
            }
        }
    }

    // ProcessShooting makes the player shoot to the current direction he's facing
    public void ProcessShooting()
    {
        if (_input.GetMouseButton(0))
            _weaponController.OnTriggerHold();
        else if (_input.GetMouseButtonUp(0))
            _weaponController.OnTriggerRelease();
    }

    void ProcessAnimations()
    {
        if (Health < 1)
            return;

        // Pre calc
        float leftRightDirection = 0;
        if (_isRunning)
        {
            float angleDiff = Vector3.Cross(_prevForward, new Vector3(transform.forward.x, 0, transform.forward.z)).y;

            angleDiff = Mathf.Sign(angleDiff);

            float step = .1f;

            if (angleDiff > _prevLeftRightDirection)
            {
                leftRightDirection = _prevLeftRightDirection + step;
                if (leftRightDirection > angleDiff)
                    leftRightDirection = angleDiff;
            }
            else
            {
                leftRightDirection = _prevLeftRightDirection - step;
                if (leftRightDirection < angleDiff)
                    leftRightDirection = angleDiff;
            }
        }

        _prevLeftRightDirection = leftRightDirection;
        _prevForward = transform.forward;
        _prevForward.y = 0;

        _animator.SetFloat("Speed", _isRunning ? 1 : 0);
        _animator.SetBool("IsRunning", _isRunning);
        _animator.SetFloat("LeftRightDirection", leftRightDirection);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Shoot");
        }

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

        if (Health == 0)
        {
            if (GameManager.PlayerKilled != null)
                GameManager.PlayerKilled(enemy, this);
            // Desaturate
            _teamC.IsKilled();
            _isRunning = false;
            PlayerIndicator.gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("PlayerDead");
            RagDoll.Ragdolled = true;
            CanMove = false;
            CanShoot = false;
        }
    }

    // IsAlive returns whether or not the player is still alive.
    public bool IsAlive()
    {
        return Health > 0;
    }
}
