using UnityEngine;

public class Bird : MonoBehaviour
{
    public float jumpPower = 7;

    private Dna dna;
    private int currentWallIndex = 0;
    private NeuralNetworkBird neuralNetworkBird;
    private Rigidbody2D birdRigidbody2D;

    private void Awake()
    {
        currentWallIndex = 0;
        neuralNetworkBird = GetComponent<NeuralNetworkBird>();

        //Aleatoriedad de colores de los Skarmory
        birdRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

    }

    public void SetDna(Dna dna)
    {
        this.dna = dna;
    }

    void Update()
    {
        Vector2 currentWallPos = GetCurrentWallPos();

        float NeuralNetworkResult = neuralNetworkBird
             .GetNeuralNetworkResult(dna, currentWallPos);

        if (NeuralNetworkResult > 0.5)
        {
            Jump();
        }
    }

    private Vector2 GetCurrentWallPos()
    {
        GameObject currentWall =
             GameManager.singleton.GetWallByIndex(currentWallIndex);

        Vector2 currentWallPos = currentWall.transform.position;
        currentWallPos.x += 0.8f;

        if (CheckIfCrossTheCurrentWall(currentWallPos))
        {
            currentWallIndex++;
            currentWallPos = currentWall.transform.position;
            currentWallPos.x += 0.8f;
        }

        return currentWallPos;
    }

    private bool CheckIfCrossTheCurrentWall(Vector2 currentWallPos)
    {
        return currentWallPos.x - transform.position.x <= 0;
    }

    //Permite volar a los Skarmory ejerciendo una fuerza dada por la variable jumpPower

    private void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
        //transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);

    }

    //Si el Skarmory choca con un objeto con el tag Wall se ejecuta Death

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Death();
        }
    }

    private void Death()
    {
        GameManager.singleton.numberOfFlappyBird--;
        dna.fitness = GameManager.singleton.currentFitness;
        Destroy(gameObject);
    }
}
