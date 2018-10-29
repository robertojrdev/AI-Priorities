using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Ours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Agent : MonoBehaviour
    {
        //threshold to limit priority
        public float velocityThreshold = 0.2f;
        public float rotationThreshold = 0.2f;

        private Rigidbody2D rb;
        private Dictionary<int, List<Steering>> priorityGroups = new Dictionary<int, List<Steering>>();

        private void Start()
        {
            AgentManager.AddAgent(this);
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        public void UpdateAgent()
        {
            //get priority steering and reset
            Steering steering = GetPrioritySteering();
            priorityGroups.Clear();

            float rotation = steering.rotation;
            Vector2 velocity = steering.velocity * Time.fixedDeltaTime;

            //apply steering
            rb.rotation = rotation;
            rb.MovePosition(rb.position + velocity);
        }

        public void AddSteering(Steering steering, int priority)
        {
            //create a priority group if it does not exist
            if (!priorityGroups.ContainsKey(priority))
            {
                priorityGroups.Add(priority, new List<Steering>());
            }

            //add to the priority's steering pool 
            priorityGroups[priority].Add(steering);
        }

        private Steering GetPrioritySteering()
        {
            //sort groups by priority
            priorityGroups = priorityGroups.OrderBy(key => key.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            
            Steering returnSteering = new Steering();

            //loop through each priority group
            foreach (var priorityGroup in priorityGroups.Values)
            {
                //reset each group
                returnSteering = new Steering();

                //loop in priority's steering pool
                foreach (var singleSteering in priorityGroup)
                {
                    returnSteering.velocity += singleSteering.velocity;
                    returnSteering.rotation += singleSteering.rotation;
                }

                //if this priority level pass in threshold value return this
                if (returnSteering.rotation >= rotationThreshold || returnSteering.velocity.magnitude >= velocityThreshold)
                    return returnSteering;
            }

            // return the lower priority steering if any other pass thrasholder value or
            // the default if any behavior is active
            return returnSteering;
        }

        private Vector2 AngleToVector2(float angle)
        {
            Vector2 direction = new Vector2();
            float x = Vector2.right.x;
            float y = Vector2.right.y;
            direction.x = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
            direction.y = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);

            return direction;
        }

        private void OnDestroy()
        {
            AgentManager.RemoveAgent(this);
        }
    }
}

