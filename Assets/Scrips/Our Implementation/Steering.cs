using UnityEngine;

namespace Ours
{
    public struct Steering
    {
        public float rotation;
        public Vector2 velocity;

        public static Steering operator* (Steering s, float val)
        {
            Steering steering = new Steering();
            steering.rotation = s.rotation * val;
            steering.velocity = s.velocity * val;

            return steering;
        }
    }
}