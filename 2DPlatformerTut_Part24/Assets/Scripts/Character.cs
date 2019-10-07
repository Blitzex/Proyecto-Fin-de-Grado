using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Contiene las funciones principales de todos los personajes del juego.
// "protected" permite que las funciones sean usadas por este script y por los que heredan de él.
// "asbtract" indica que "Character" es como una familia que contiene todos los datos de los personajes, ya sea el enemigo o el que controlas.
public abstract class Character : MonoBehaviour {

    // Indica la posición donde se instancia el cuchillo.
    [SerializeField]
    protected Transform knifePos;

    // Indica la velocidad de movimiento del personaje.
    [SerializeField]
    protected float movementSpeed;

    // Indica si el personaje está girado a la derecha.
    protected bool facingRight;

    // Instancia el cuchillo.
    [SerializeField]
    private GameObject knifePrefab;

    // Indica la vida del personaje.
    [SerializeField]
    protected Stat healthStat;

    // Collider de la espada del personaje.
    [SerializeField]
    private EdgeCollider2D swordCollider;

    // Lista de objetos que producen daño al personaje.
    [SerializeField]
    private List<string> damageSources;

    // Indica si el personaje está muerto.
    public abstract bool IsDead { get; }

    // Indica si el personaje puede atacar.
    public bool Attack { get; set; }

    // Indica si el personaje está recibiendo daño.
    public bool TakingDamage { get; set; }

    // Referencia al animator del personaje.
    public Animator MyAnimator { get; private set; }

    // Propiedad necesaria para el swordcollider.
    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    // Use this for initialization
    public virtual void Start ()
    {
        facingRight = true;

        MyAnimator = GetComponent<Animator>();

        healthStat.Initialize();  // Inicializa la vida del personaje.
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    // Hace que el personaje pueda recibir daño.
    public abstract IEnumerator TakeDamage();

    // Maneja la muerte del personaje.
    public abstract void Death();

    // Cambia la dirección en la que mira el personaje.
    public virtual void ChangeDirection()
    {
        // Cambia el valor de la booleana de "facingRight" a negativo.
        facingRight = !facingRight;

        // Voltea al personaje cambiando su escala.
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    // Se encarga del lanzamiento del cuchillo
    public virtual void ThrowKnife(int value)
    {
        if (facingRight) // Si está mirando a la derecha lanza el cuchillo a la derecha.
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else // Si está mirando a la izquierda lo lanza a la izquierda.
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    // Realiza un ataque melee.
    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
        Vector3 tmpPos = swordCollider.transform.position;
        swordCollider.transform.position = new Vector3(swordCollider.transform.position.x + 0,01, swordCollider.transform.position.y);
        swordCollider.transform.position = tmpPos;
    }
 
    // Realiza una acción dependiendo de con que tipo de objeto colisione el personaje.
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto produce daño (el cuchillo, por ejemplo), inicia la corrutina de "TakeDamage".
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
