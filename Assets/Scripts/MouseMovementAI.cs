using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MouseMovementAI : MonoBehaviour
{
    [SerializeField] Food foodObject;
    [SerializeField] LayerMask avoidHitLayerMask;
    [SerializeField] float avoidHitDistance = 3f;
    [SerializeField] float randomDirChangePerc = 50f;
    [SerializeField] float dirChangeCooldownTime = 3f;

    private Rigidbody rigidBody;
    private Vector2 moveInput;
    private float currCooldownTime;
    private float speed = 2f;
    private float turnSpeed = 70f;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        speed = foodObject.speed;
        turnSpeed = foodObject.turnSpeed;
    }

    void Update()
    {
        if (currCooldownTime > 0f)
            currCooldownTime -= Time.deltaTime;
        else
        {
            currCooldownTime = 0f;
            moveInput.x = 0;
        }
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        moveInput = CheckIfGoingToHitAndTurn(moveInput);

        if (moveInput.x == 0 && currCooldownTime == 0f)
            moveInput = RandomDirectionChange();

        if (moveInput.x != 0)
        {
            Vector3 rotationEuler = new Vector3(0, turnSpeed * -moveInput.x * Time.fixedDeltaTime, 0);
            deltaRotation = Quaternion.Euler(rotationEuler);

            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }

        Vector3 moveForward = transform.forward * speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + moveForward);
    }

    Vector2 CheckIfGoingToHitAndTurn(Vector2 currInput)
    {
        RaycastHit hit;
        Vector3 direction = Vector3.forward;

        //Debug.DrawRay(transform.position, transform.TransformDirection(direction) * avoidHitDistance, Color.red);

        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, avoidHitDistance, avoidHitLayerMask))
            if (Random.Range(0, 1) == 0)
                return new Vector2(1, 0);
            else
                return new Vector2(-1, 0);

        Vector3 rotatedDirection = (Vector3.forward + Vector3.right).normalized;
        //Debug.DrawRay(transform.position, transform.TransformDirection(rotatedDirection) * avoidHitDistance, Color.blue);

        if (Physics.Raycast(transform.position, transform.TransformDirection(rotatedDirection), out hit, avoidHitDistance, avoidHitLayerMask))
            return new Vector2(1, 0);

        rotatedDirection = (Vector3.forward - Vector3.right).normalized;
        //Debug.DrawRay(transform.position, transform.TransformDirection(rotatedDirection) * avoidHitDistance, Color.yellow);

        if (Physics.Raycast(transform.position, transform.TransformDirection(rotatedDirection), out hit, avoidHitDistance, avoidHitLayerMask))
            return new Vector2(-1, 0);

        return currInput;
    }

    Vector2 RandomDirectionChange()
    {
        if(Random.Range(0,100) <= randomDirChangePerc)
        {
            currCooldownTime = dirChangeCooldownTime;

            if (Random.Range(0, 100) <= 50)
                return new Vector2(1, 0);
            else
                return new Vector2(-1, 0);
        }

        return new Vector2(0, 0);

    }
}
