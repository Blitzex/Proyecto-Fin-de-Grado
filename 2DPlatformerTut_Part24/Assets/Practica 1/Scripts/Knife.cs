using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

// La clase del objeto arrojadizo, el cuchillo.
public class Knife : MonoBehaviour
{
    // Indica la velocidad a la que va el cuchillo.
    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

	// Use this for initialization
	void Start () {

        myRigidbody = GetComponent<Rigidbody2D>(); // Le asigna un rigidbody al cuchillo.
	}

    void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed; // Determina la velocidad del cuchillo.
             
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Destruye el cuchillo cuando se deja de ver en pantalla.
    }
}
