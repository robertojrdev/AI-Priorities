using UnityEngine;

public class SeekBehavior : MonoBehaviour 
{
    public float speed = 1;
    public float minDistance = 1;
    private Transform target;
    private LookBehavior lookBeahvior;

    private void Start()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Target").transform);
        lookBeahvior = GetComponent<LookBehavior>();
    }

    private void Update()
    {
        DoSeek();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void DoSeek()
    {
        if (!target)
            return;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= minDistance)
            return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if(lookBeahvior)
        {
            lookBeahvior.SetLookDirection(direction);
        }
    }
}
