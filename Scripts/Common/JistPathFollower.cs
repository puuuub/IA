using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class JistPathFollower : PathFollower
{
    float distanceTravelled_;


    public float TimeTravelled { get { return distanceTravelled_ / speed; } }

    public float GetSecondsByDistance(float dist)
    {
        return dist / speed;
    }

    public float GetSecondsByPositon(Vector3 position)
    {
        float dist = pathCreator.path.GetClosestDistanceAlongPath(position);
        return dist / speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            //distanceTravelled_ = StartDistance;
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled_ += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled_, endOfPathInstruction);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled_, endOfPathInstruction);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled_ = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
