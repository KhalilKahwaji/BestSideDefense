using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Vector2 dir = Vector2.left; // default: move left

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDir)
    {
        if (newDir == Vector2.zero) return;
        dir = newDir.normalized;
    }
}