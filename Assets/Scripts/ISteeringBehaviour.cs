using UnityEngine;

public interface ISteeringBehaviour
{
    float Weight { get; }
    int Priority { get; }
    SteeringOutput GetSteering(GameObject target);
}

