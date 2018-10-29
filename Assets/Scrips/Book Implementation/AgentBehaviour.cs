﻿using UnityEngine;

[RequireComponent(typeof(Agent))]
public class AgentBehaviour : MonoBehaviour 
{
    public GameObject target;
    [SerializeField]protected Agent agent;
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;    public int priority = 1;
    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering(), priority);
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }

    public Vector3 GetOriAsVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
