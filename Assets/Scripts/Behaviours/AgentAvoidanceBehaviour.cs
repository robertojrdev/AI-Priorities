using System.Collections.Generic;
using UnityEngine;

public class AgentAvoidanceBehaviour : SteeringBehaviour
{
    public float radius = 1;
    public float decayCoeficient = 1;

    public override SteeringOutput GetSteering(GameObject target)
    {
        // Initialize linear and angular forces to zero
        Vector2 linear = Vector2.zero;
        float angular = 0f;

        //get agents directions
        Vector2[] directions = GetPotentialAgentsDirections();
        
        // Get repulsion force and apply to linear
        Vector2 repulsion = GetRepulsionVector(directions);
        linear = repulsion;

        // Output the steering
        return new SteeringOutput(linear, angular);
    }

    private Vector2 GetRepulsionVector(Vector2[] directions)
    {
        Vector2 repulsion = Vector2.zero;

        foreach (var dir in directions)
        {
            float distance = dir.magnitude;

            //check if is inside the repulsion radius
            if (distance > radius)
                continue;

            // Calculate repulison strength
            float strength = CalculateStrength(distance);

            // Add to final repulsion force
            repulsion += dir.normalized * strength;
        }

        return repulsion;
    }

    private Vector2[] GetPotentialAgentsDirections()
    {
        List<Vector2> directions = new List<Vector2>();
        foreach (var agent in DynamicAgent.DynamicAgents)
        {
            //remove this agent and compute agents with same tag
            if (agent == this.agent || !agent.CompareTag(this.agent.tag))
                continue;

            Vector2 dir = transform.position - agent.transform.position;
            directions.Add(dir);
        }

        return directions.ToArray();
    }

    private float CalculateStrength(float distance)
    {
        //use the square law
        return Mathf.Min(decayCoeficient / (distance * distance), agent.maxAccel);
    }

    //to see radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
