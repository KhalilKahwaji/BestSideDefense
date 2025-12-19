using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    public enum Dir { Right, Left, Up, Down }
    [SerializeField] private Dir direction;

    private Vector2 GetVector()
    {
        return direction switch
        {
            Dir.Right => Vector2.right,
            Dir.Left => Vector2.left,
            Dir.Up => Vector2.up,
            Dir.Down => Vector2.down,
            _ => Vector2.left
        };
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyMovement>();
        if (enemy != null)
            enemy.SetDirection(GetVector());
    }
}
