using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour
{
    /// <summary>
    /// A reference to the ChildPlatform's transform
    /// </summary>
    [SerializeField]
    private Transform platformTransform;

    /// <summary>
    /// The transform of the first destination
    /// </summary>
    [SerializeField]
    private Transform transformB;

    /// <summary>
    /// Position A
    /// </summary>
    private Vector3 posA;

    /// <summary>
    /// Position B
    /// </summary>
    private Vector3 posB;

    /// <summary>
    /// This variable holds the value of the current destination
    /// </summary>
    private Vector3 nextPos;

    /// <summary>
    /// The movement speed of the Platform
    /// </summary>
    [SerializeField]
    private float movementSpeed;

    // Use this for initialization
    void Start()
    {
        //Sets pos a equal to the platform's startpostion
        posA = platformTransform.localPosition;
        posB = transformB.localPosition;
        nextPos = posB;
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the platform
        platformTransform.localPosition = Vector3.MoveTowards(platformTransform.localPosition, nextPos, movementSpeed * Time.deltaTime);

        //Checks if we need to change destitaion from a to b or b to a
        if (Vector3.Distance(platformTransform.localPosition, nextPos) <= 0.1)
        {
            ChangeDestination();
        }
    }

    /// <summary>
    /// Changes the platforms destination
    /// </summary>
    private void ChangeDestination()
    {
        nextPos = nextPos != posA ? posA : posB;
    }

    /// <summary>
    /// If we collide with the player
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.layer = 11; //Change the player's layer to platform layer
            other.transform.SetParent(platformTransform); //Change the players parent to the platform
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //remove the players as a child
            other.transform.SetParent(null);
        }
    }
}
