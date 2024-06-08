using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AbilityType
{
    None,
    Boost,
    Rocket,
    Freeze,
    Bomb,
    Invincible
}

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public int maxJumps = 2;
    public Transform respawnPoint;
    public bool canDoubleJump = true;

    [SerializeField] private AbilityType _equippedAbility;
    [SerializeField] private AbilityData _abilityData;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private bool isGrounded;
    private int remainingJumps;
    private InputSystem_Actions controls;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new InputSystem_Actions();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => OnJump();
        controls.Player.Interact.performed += ctx => OnAbilityPressed();
    }

    void OnEnable() {
        controls.Player.Enable();
    }

    void OnDisable() {
        controls.Player.Disable();
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        if (isGrounded) {
            remainingJumps = maxJumps;
        }

        if (transform.position.y < -10f) {
            Die();
        }

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    void OnJump() {
        if (isGrounded || (canDoubleJump && remainingJumps > 0)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }
    }

    void Die() {
        Debug.Log("You died :(");
        Respawn();
    }

    void Respawn() {
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike")) {
            Die();
        } else if (collision.gameObject.CompareTag("Enemy")) {
            if (transform.position.y > collision.transform.position.y + 0.5f) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            } else {
                Die();
            }
        }
    }

    public void DebugUseAbility(int ability)
    {
        _equippedAbility = (AbilityType) ability;
        OnAbilityPressed();
    }
    
    private void OnAbilityPressed()
    {
        float power = _abilityData.GetPower(_equippedAbility);
        
        if (_equippedAbility == AbilityType.Boost)
            DoHorizontalBoost(power);
        else if (_equippedAbility == AbilityType.Rocket)
            DoVerticalBoost(power);
        else if (_equippedAbility == AbilityType.Freeze)
            DoFreeze(power);
        else if (_equippedAbility == AbilityType.Bomb)
            DoBomb(power);
        else if (_equippedAbility == AbilityType.Invincible)
            DoInvicibility(power);
    }

    private void DoHorizontalBoost(float power)
    {
        Debug.Log("Boost!");
    }
    
    private void DoVerticalBoost(float power)
    {
        Debug.Log("Rocket!");
    }
    
    private void DoFreeze(float time)
    {
        Debug.Log($"Freeze for {time} seconds!");
    }
    
    private void DoBomb(float power)
    {
        Debug.Log("Bomb!");
    }
    
    private void DoInvicibility(float time)
    {
        Debug.Log($"Invincible for {time} seconds!");
    }
}