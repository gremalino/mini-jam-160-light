using UnityEngine;
using System.Collections;

public class Crusher : MonoBehaviour {

    public float moveDistance = 5f;
    public float moveDuration = 2f;
    public float pauseDuration = 0.5f;
    public float initialDelay = 1f;

    private Vector3 startingPosition;
    private bool movingUp = true;

    void Start() {
        startingPosition = transform.position;
        StartCoroutine(MoveCrusher());
    }

    IEnumerator MoveCrusher() {

        yield return new WaitForSeconds(initialDelay);

        while (true) {
            yield return new WaitForSeconds(pauseDuration);
            yield return MoveToPosition(startingPosition + Vector3.up * moveDistance, moveDuration);
            yield return new WaitForSeconds(pauseDuration);
            yield return MoveToPosition(startingPosition, moveDuration);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration) {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}