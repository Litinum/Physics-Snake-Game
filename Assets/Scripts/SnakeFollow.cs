using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class SnakeFollow : MonoBehaviour
{
    public GameObject targetToFollow;
    [SerializeField] float followDistance = 1.05f;
    [SerializeField] float minStepDistance = 0.05f;

    private Rigidbody rigidbody;
    private List<Vector3> pathHistory;
    private List<Quaternion> rotationHistory;
    private Controller snakeController;

    void Start()
    {
        pathHistory = new List<Vector3>();
        rotationHistory = new List<Quaternion>();
        rigidbody = GetComponent<Rigidbody>();

        if(targetToFollow != null)
        {
            pathHistory.Add(targetToFollow.transform.position);
            rotationHistory.Add(targetToFollow.transform.rotation);
        }

        snakeController = transform.parent.GetChild(0).GetComponent<Controller>();
    }

    private void FixedUpdate()
    {
        if (targetToFollow == null)
            return;

        if (Vector3.Distance(targetToFollow.transform.position, pathHistory[pathHistory.Count - 1]) > minStepDistance)
        {
            pathHistory.Add(targetToFollow.transform.position);
            rotationHistory.Add(targetToFollow.transform.rotation);
        }

        while(pathHistory.Count > 1)
        {
            float distanceToNextPoint = Vector3.Distance(rigidbody.position, pathHistory[0]);
            float totalTrailLength = distanceToNextPoint;

            for (int i = 0; i < pathHistory.Count - 1; i++)
            {
                totalTrailLength += Vector3.Distance(pathHistory[i], pathHistory[i + 1]);
            }

            if (totalTrailLength <= followDistance)
                break;

            rigidbody.MovePosition(Vector3.MoveTowards(rigidbody.position, pathHistory[0], Time.fixedDeltaTime * (snakeController.speed * 1.5f)));
            rigidbody.rotation = rotationHistory[0];

            if (Vector3.Distance(rigidbody.position, pathHistory[0]) < minStepDistance)
            {
                pathHistory.RemoveAt(0);
                rotationHistory.RemoveAt(0);
            }
            else
                break;
        }
    }
}
