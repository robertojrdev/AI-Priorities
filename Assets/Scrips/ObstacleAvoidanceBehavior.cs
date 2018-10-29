using UnityEngine;

public class ObstacleAvoidanceBehavior : MonoBehaviour 
{
    public float distance = 3;
    public int raysCount = 8;
    public float smoothSpeed = 1;
    public float avoidForce = 1; 

    private void Update()
    {
        Avoid();
    }

    private void Avoid()
    {
        Vector2 newPosition = transform.position + (Vector3)GetCollisionResultant();

        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);
    }

    private Vector2 GetCollisionResultant()
    {
        Vector2 collisionNormalAverage = Vector2.zero;
        foreach (var hit in GetAllHits())
        {
            Vector2 normal = hit.normal;
            float collisionDistance = Vector2.Distance(hit.point, transform.position);
            normal *= distance - collisionDistance;

            collisionNormalAverage += normal;
        }

        return collisionNormalAverage;
    }

    private RaycastHit2D[] GetAllHits()
    {
        RaycastHit2D[] hit = new RaycastHit2D[raysCount];

        for (int i = 0; i < raysCount; i++)
        {
            float angle = (360 / raysCount) * i;
            Vector2 direction = GetAngleVector(angle);

            hit[i] = Physics2D.Raycast(transform.position, direction, distance);
        }

        return hit;
    }

    private Vector2 GetAngleVector(float angle)
    {
        angle = Mathf.Deg2Rad * angle;
        Vector2 dir = new Vector2();
        float x = transform.right.x;
        float y = transform.right.y;
        dir.x = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
        dir.y = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);

        return dir;
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < raysCount; i++)
        {
            float angle = (360 / raysCount) * i;
            Vector2 direction = GetAngleVector(angle);
            Gizmos.DrawRay(transform.position, direction * distance);
        }
    }

}
