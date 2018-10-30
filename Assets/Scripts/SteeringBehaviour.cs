using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour, ISteeringBehaviour {

    [SerializeField] protected float weight = 1;
    [SerializeField] protected int priority = 1;

    protected DynamicAgent agent;
    protected Rigidbody2D rb;

    public float Weight { get { return weight; } }
    public int Priority { get { return priority; } }

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<DynamicAgent>();
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract SteeringOutput GetSteering(GameObject target);
}
