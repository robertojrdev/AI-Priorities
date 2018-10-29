using UnityEngine;

public class Character : MonoBehaviour 
{

}


/*
    blendSteering groups[]
    float psilon
    
    void getSteering()
    {
        for each(var group in groups)
        {
            // Create the steering structure for accumulation
            steering = group.getSteering()
    
            // Check if we’re above the threshold, if so return
            if (steering.linear.length() > epsilon || abs(steering.angular) > epsilon)
                return steering
        }
    
        // If we get here, it means that no group had a large
        // enough acceleration, so return the small
        // acceleration from the final group.
        return steering
    }


    

*/
