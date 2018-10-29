using System.Collections.Generic;
using UnityEngine;

namespace Ours
{
    public class AgentManager : MonoBehaviour
    {
        private static AgentManager _instance;
        public static AgentManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = new GameObject().AddComponent<AgentManager>();

                return _instance;
            }
        }

        public List<AgentBehaviour> behaviourPool { get; private set; }
        public List<Agent> agentPool { get; private set; }

        private void Awake()
        {
            //block multiple instances
            if (_instance && _instance != this)
                Destroy(this);

            behaviourPool = new List<AgentBehaviour>();
            agentPool = new List<Agent>();
        }

        private void Update()
        {
            UpdateBehaviours();
            UpdateAgents();
        }

        private void UpdateBehaviours()
        {
            foreach (var behaviour in behaviourPool)
            {
                behaviour.SendSteeringToAgent();
            }
        }

        private void UpdateAgents()
        {
            foreach (var agent in agentPool)
            {
                agent.UpdateAgent();
            }
        }

        public static List<AgentBehaviour> GetBehaviours()
        {
            return Instance.behaviourPool;
        }

        public static List<Agent> GetAgents()
        {
            return Instance.agentPool;
        }

        public static void AddAgent(Agent agent)
        {
            Instance.agentPool.Add(agent);
        }

        public static void AddBehaviour(AgentBehaviour behaviour)
        {
            Instance.behaviourPool.Add(behaviour);
        }

        public static void RemoveAgent(Agent agent)
        {
            if (!_instance)
                return;

            if(Instance.agentPool.Contains(agent))
                Instance.agentPool.Remove(agent);
        }

        public static void RemoveBehaviour(AgentBehaviour behaviour)
        {
            if (!_instance)
                return;

            if (Instance.behaviourPool.Contains(behaviour))
                Instance.behaviourPool.Remove(behaviour);
        }
    }
}