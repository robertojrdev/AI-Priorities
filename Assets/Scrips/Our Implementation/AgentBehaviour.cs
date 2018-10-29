using UnityEngine;

namespace Ours
{
    [RequireComponent(typeof(Agent))]
    public abstract class AgentBehaviour : MonoBehaviour
    {
        public int priority = 1;
        protected Agent agent;

        private void Awake()
        {
            agent = GetComponent<Agent>();
            AgentManager.AddBehaviour(this);
        }

        public void SendSteeringToAgent()
        {
            agent.AddSteering(GetSteering(), priority);
        }

        public abstract Steering GetSteering();

        protected virtual void OnDestroy()
        {
            AgentManager.RemoveBehaviour(this);
        }
    }
}