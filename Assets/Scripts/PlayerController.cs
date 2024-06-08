using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AbilityType {
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

    private Dictionary<AbilityType, float> abilityTimers = new Dictionary<AbilityType, float>();

    [SerializeField] private float currentXVelocity;
    [SerializeField] private float currentYVelocity;

    private bool isHorizontalBoosting;
    private bool isVerticalBoosting;

    private float lastHorizontalDirection;

    [SerializeField] private float speedThreshold = 15f;
    [SerializeField] private float decelerationStrength = 0.5f;

    private bool isInvincible;
    private bool isCollidingWithDanger;

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

        if (!isHorizontalBoosting) {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }

        ApplyDeceleration();

        if (!isVerticalBoosting) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        currentXVelocity = rb.velocity.x;
        currentYVelocity = rb.velocity.y;

        if (moveInput.x != 0) {
            lastHorizontalDirection = Mathf.Sign(moveInput.x);
        }

        List<AbilityType> keys = new List<AbilityType>(abilityTimers.Keys);
        foreach (AbilityType ability in keys) {
            abilityTimers[ability] -= Time.deltaTime;
            if (abilityTimers[ability] <= 0) {
                abilityTimers.Remove(ability);
            }
        }
    }

    void OnJump() {
        if (isGrounded || (canDoubleJump && remainingJumps > 0)) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }
    }

    void Die() {
        if (isInvincible) {
            Debug.Log("Player can't die, go wild!");
            return;
        }

        Debug.Log("You died :(");
        Respawn();
    }

    void Respawn() {
        transform.position = respawnPoint.position;
        rb.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy")) {
            isCollidingWithDanger = true;
            if (!isInvincible) {
                Die();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Enemy")) {
            isCollidingWithDanger = false;
        }
    }

    public void DebugUseAbility(int ability) {
        _equippedAbility = (AbilityType) ability;
        OnAbilityPressed();
    }

    private void OnAbilityPressed() {
        if (abilityTimers.ContainsKey(_equippedAbility)) {
            Debug.Log($"{_equippedAbility} is on cooldown!");
            return;
        }

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
            DoInvincibility(power);
    }

    private void DoHorizontalBoost(float power) {
        Debug.Log("Boost!");

        float dashDirection = moveInput.x == 0 ? lastHorizontalDirection : Mathf.Sign(moveInput.x);

        rb.velocity = new Vector2(dashDirection * power, rb.velocity.y);

        StartCooldown(AbilityType.Boost);

        isHorizontalBoosting = true;
        StartCoroutine(EndHorizontalBoostAfterTime(0.5f));
    }

    private void DoVerticalBoost(float power) {
        Debug.Log("Rocket!");

        rb.velocity = new Vector2(rb.velocity.x, power);

        StartCooldown(AbilityType.Rocket);

        isVerticalBoosting = true;
        StartCoroutine(EndVerticalBoostAfterTime(0.5f));
    }

    private void DoFreeze(float time) {
        Debug.Log($"Freeze for {time} seconds!");
        StartCooldown(AbilityType.Freeze);
    }

    private void DoBomb(float power) {
        Debug.Log("Bomb!");
        StartCooldown(AbilityType.Bomb);
    }

    private void DoInvincibility(float time) {
        Debug.Log($"Invincible for {time} seconds!");

        isInvincible = true;

        StartCoroutine(InvincibilityDuration(time));

        StartCooldown(AbilityType.Invincible);
    }

    private IEnumerator InvincibilityDuration(float time) {
        yield return new WaitForSeconds(time);
        isInvincible = false;
        if (isCollidingWithDanger) {
            Die();
        }
    }

    private void StartCooldown(AbilityType ability) {
        float cooldown = _abilityData.GetCooldown(ability);
        abilityTimers[ability] = cooldown;
    }

    private void ApplyDeceleration() {
        if (Mathf.Abs(rb.velocity.x) > speedThreshold) {
            float deceleration = decelerationStrength * Time.deltaTime * Mathf.Sign(rb.velocity.x);
            float newVelocityX = rb.velocity.x - deceleration;

            if (Mathf.Abs(newVelocityX) < speedThreshold) {
                newVelocityX = speedThreshold * Mathf.Sign(rb.velocity.x);
            }

            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
        }
    }

    private IEnumerator EndHorizontalBoostAfterTime(float duration) {
        yield return new WaitForSeconds(duration - 0.1f);

        float elapsedTime = 0f;
        float decelerationDuration = 0.1f;
        float targetSpeed = 15f;
        float initialSpeed = Mathf.Abs(rb.velocity.x);

        while (elapsedTime < decelerationDuration) {
            elapsedTime += Time.deltaTime;
            float newSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / decelerationDuration);
            float dashDirection = rb.velocity.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(dashDirection * newSpeed, rb.velocity.y);
            yield return null;
        }

        isHorizontalBoosting = false;
    }

    private IEnumerator EndVerticalBoostAfterTime(float duration) {
        yield return new WaitForSeconds(duration);
        isVerticalBoosting = false;
    }
}