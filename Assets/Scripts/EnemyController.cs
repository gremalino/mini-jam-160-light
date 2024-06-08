using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Rigidbody2D rb;
    public Transform[] patrolPoints;
    public float speed = 2f;
    private int currentPointIndex = 0;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Patrol();
    }

    void Patrol() {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f) {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (collision.transform.position.y > transform.position.y + 0.5f) {
                Die();
            }
        }
    }

    void Die() {
        Debug.Log("Enemy unalived :O");
        Destroy(gameObject);
    }
}