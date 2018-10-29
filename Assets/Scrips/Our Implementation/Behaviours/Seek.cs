﻿using UnityEngine;

namespace Ours
{
    public class Seek : AgentBehaviour
    {
        public float speed = 1;
        public float minDist = 1;
        public Transform target;

        public override Steering GetSteering()
        {
            Steering steering = new Steering();

            if (!target)
                return steering;

            //give the direction of target
            Vector2 direction = target.position - transform.position;
            //if is min distance return
            float dist = direction.magnitude;
            if (dist < minDist)
                return steering;

            //apply rotation and velocity
            steering.rotation = Vector2.SignedAngle(Vector3.right, direction);
            steering.velocity = direction.normalized * speed;

            return steering;
        }
    }
}