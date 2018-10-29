using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Agent : MonoBehaviour 
{
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;
    private Rigidbody aRigidBody;
    public float priorityThreshold = 0.2f;
    [SerializeField]private Dictionary<int, List<Steering>> groups;

    void Start()
    {
        groups = new Dictionary<int, List<Steering>>();
        velocity = Vector3.zero;
        steering = new Steering();
        aRigidBody = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        if (aRigidBody == null)
            return;

        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        // we need to limit the orientation values
        // to be in the range (0 - 360)
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }

    public virtual void LateUpdate()
    {
        if (aRigidBody == null)
            return;

        // funnelled steering through priorities
        steering = GetPrioritySteering();
        groups.Clear();


        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }

        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        // The ForceMode will depend on what you want to achieve
        // We are using VelocityChange for illustration purposes
        aRigidBody.AddForce(displacement, ForceMode.VelocityChange);
        Vector3 orientationVector = OriToVec(orientation);
        aRigidBody.rotation = Quaternion.LookRotation(orientationVector, Vector3.up);

        steering = new Steering();
    }

    public void SetSteering(Steering steering, int priority)
    {
        if (!groups.ContainsKey(priority))
        {
            groups.Add(priority, new List<Steering>());
        }
        groups[priority].Add(steering);
    }    private Steering GetPrioritySteering()
    {
        Steering steering = new Steering();
        float sqrThreshold = priorityThreshold * priorityThreshold;

        //sort groups
        groups = groups.OrderBy(key => key.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

        string debug = "Debug: ";
        foreach (List<Steering> group in groups.Values)
        {
            steering = new Steering();
            foreach (Steering singleSteering in group)
            {
                steering.linear += singleSteering.linear;
                steering.angular += singleSteering.angular;
            }

            /*
            if (steering.linear.sqrMagnitude > sqrThreshold || Mathf.Abs(steering.angular) > priorityThreshold)
            {
                return steering;
            }
            */
        }
        return steering;
    }    
    public Vector3 OriToVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }

}
