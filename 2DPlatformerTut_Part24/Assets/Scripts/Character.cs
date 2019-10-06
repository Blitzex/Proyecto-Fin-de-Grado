using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Contiene las funciones de todos los personajes de la clase "Character" del juego.
//Las funciones "protected" permiten que su contenido sea usado por otros scripts que hereden de "Character", además del propio script.
//"abstract" impide que el script sea asignado como un Gameobject y define una familia entera, en este caso "Character" define a todos los personajes del juego.

public abstract class Character : MonoBehaviour
{

    [SerializeField]
    protected Transform knifePos; //Indica la posición en la que aparecerá el cuchillo del personaje.

    protected float movementSpeed; //Indica la velocidad de movimiento del personaje.

    protected bool facingRight; //Indica si el personaje está mirando a la derecha.

    [SerializeField]
    private GameObject knifePrefab; //Instancia el prefab del cuchillo.

    [SerializeField]
    protected Stat healthStat; //Indica la vida del personaje.

    [SerializeField]
    private EdgeCollider2D swordCollider; //Collider de la espada.

    [SerializeField]
    private List<string> damageSources; //Lista de cosas que dañan a los personajes.

    public abstract bool IsDead { get; } //Indica si el personaje está muerto.

    public bool Attack { get; set; } //Indica si el personaje puede atacar.

    public bool TakingDamage { get; set; } //Indica si el personaje está recibiendo daño.

    public Animator MyAnimator { get; private set; } //Referencia al animator del personaje.

    public EdgeCollider2D SwordCollider //Propiedad para tener el swordCollider.
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

        healthStat.Initialize(); //Inicializa la barra de vida del personaje.
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public abstract IEnumerator TakeDamage(); //Permite que el personaje reciba daño.

    public abstract void Death(); //Maneja la muerte de los personajes.

    public virtual void ChangeDirection() //Cambia la dirreción de los personajes.
    {
        //Cambia la booleana "facingRight" a su valor negativo.
        facingRight = !facingRight;

        //Voltea al personaje cambiando su escala.
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value) //Lanza un cuchillo.
    {
        if (facingRight) //Si está mirando a la derecha, tira el cuchillo a la derecha.
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else //Si está mirando a la izquierda, tira el cuchillo a la izquierda.
        {
            GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttack() //Hace un ataque melee.
    {
        SwordCollider.enabled = true;
        Vector3 tmpPos = swordCollider.transform.position;
        swordCollider.transform.position = new Vector3(swordCollider.transform.position.x + 0,01, swordCollider.transform.position.y);
        swordCollider.transform.position = tmpPos;
    }
 
    public virtual void OnTriggerEnter2D(Collider2D other) //Se activa cuando el personaje choca con un objeto.
    {
        //Si el objeto con el que choca es una fuente de daño ejecuta la corutina de "TakeDamage"
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
