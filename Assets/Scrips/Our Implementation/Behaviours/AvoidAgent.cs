using UnityEngine;

namespace Ours
{
    public class AvoidAgent : AgentBehaviour
    {
        public float radiusRange = 1;

        public override Steering GetSteering()
        {
            Steering steering = new Steering();

            Vector2 normalVector = GetNormalVectorOfAgentsInRange();
            steering.velocity = normalVector;
            return steering;
        }
        
        private Vector2 GetNormalVectorOfAgentsInRange()
        {
            Vector2 resultantNormal = Vector2.zero;

            foreach (var agent in AgentManager.GetAgents())
            {
                if (agent == this)
                    continue;

                Vector2 normal = transform.position - agent.transform.position;
                float distance = normal.magnitude;
                if (radiusRange > distance)
                    resultantNormal += normal;
            }

            return resultantNormal;
        }
    }
}