/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 * Author: Nuno Fachada 
 * */

using UnityEngine;
using System.Linq;
using System.Collections.Generic;

// This class defines movement for dynamic agents
public class DynamicAgent : MonoBehaviour
{
    private static List<DynamicAgent> dynamicAgents; // All agents in the scene
    public static List<DynamicAgent> DynamicAgents
    {
        get
        {
            if (dynamicAgents == null)
                dynamicAgents = new List<DynamicAgent>();

            return dynamicAgents;
        }

        private set
        {
            if (dynamicAgents == null)
                dynamicAgents = new List<DynamicAgent>();

            dynamicAgents = value;
        }
    } // Encapsulate...

    public float maxAccel; // Maximum acceleration for this agent
    public float maxSpeed; // Maximum speed for this agent
    public float maxAngularAccel; // Maximum angular acceleration for this agent
    public float maxRotation; // Maximum rotation (angular velocity) for this agent
    public float priorityThreshold; // Check if the output of the priority behaviour is enough to trigger the action
    public string targetTag; // The tag for this agent's target

    // Agent steering behaviours
    private Dictionary<int, List<ISteeringBehaviour>> steeringBehaviours = new Dictionary<int, List<ISteeringBehaviour>>();
    
    public Rigidbody2D Rb { get; private set; } // The agent's rigid body

    private void Start()
    {
        DynamicAgents.Add(this);

        // Keep reference to rigid body
        Rb = GetComponent<Rigidbody2D>();

        // Get steering behaviours defined for this agent
        ISteeringBehaviour[] behaviours = GetComponents<ISteeringBehaviour>();

        // Add behaviours to dictionary using priority as key
        foreach (var b in behaviours)
        {
            if(!steeringBehaviours.ContainsKey(b.Priority))
            {
                steeringBehaviours.Add(b.Priority, new List<ISteeringBehaviour>());
            }

            steeringBehaviours[b.Priority].Add(b);
        }
        
        // Sort stering behaviours by priority (ascending)
        steeringBehaviours = steeringBehaviours.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    // This is called every physics update
    private void FixedUpdate()
    {
        // Is there any target for me?
        GameObject target = targetTag != ""
            ? GameObject.FindWithTag(targetTag)
            : null;

        // Obtain steering behaviours
        SteeringOutput steerPriorityWeighted = GetPrioritySteeringWeighted(target);

        // Apply steering
        Rb.AddForce(steerPriorityWeighted.Linear);
        Rb.AddTorque(steerPriorityWeighted.Angular);

        // Limit speed
        if (Rb.velocity.magnitude > maxSpeed)
        {
            Rb.velocity = Rb.velocity.normalized * maxSpeed;
        }

        // Limit rotation (angular velocity)
        if (Rb.angularVelocity > maxRotation)
        {
            Rb.angularVelocity = maxRotation;
        }
    }

    private SteeringOutput GetPrioritySteeringWeighted(GameObject target)
    {
        SteeringOutput steeringOutput = new SteeringOutput();

        // threshhold for the linear
        float sqrThreshold = priorityThreshold * priorityThreshold;

        foreach (var priorityList in steeringBehaviours)
        {
            steeringOutput = new SteeringOutput();

            foreach (ISteeringBehaviour behaviour in priorityList.Value)
            {
                SteeringOutput steer = behaviour.GetSteering(target);

                steeringOutput.Linear += behaviour.Weight * steer.Linear;
                steeringOutput.Angular += behaviour.Weight * steer.Angular;
            }

            // If this priority list pass threshold test, return , else continue the loop or will return the lowest priority
            if (steeringOutput.Linear.sqrMagnitude > sqrThreshold || Mathf.Abs(steeringOutput.Angular) > priorityThreshold)
                break;
        }

        return steeringOutput;
    }

    private void OnDestroy()
    {
        DynamicAgents.Remove(this);
    }
}
