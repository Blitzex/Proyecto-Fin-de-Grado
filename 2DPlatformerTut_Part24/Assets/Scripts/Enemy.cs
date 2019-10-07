using UnityEngine;
using System.Collections;
using System;

// La clase del personaje enemigo.
public class Enemy : Character
{

    // El estado actual del enemigo, cambiar esto afectará a su comportamiento.
    private IEnemyState currentState;

    // Indica el objetivo a por el que deberá ir el enemigo, bajo la etiqueta "target".
    public GameObject Target { get; set; }

    // Indica el rango del ataque melee del enemigo, y cuando debe dejar de usar cuchillos y usar la espada (por proximidad al personaje).
    [SerializeField]
    private float meleeRange;

    // Indica el rango de lanzamiento del enemigo, es decir, desde que punto puede empezar a lanzar cuchillos al personaje.
    [SerializeField]
    private float throwRange;

    private Vector3 startPos; // Posicion inicial del enemigo al darle al Play.

    [SerializeField]
    public Transform leftEdge; // Indica la zona máxima del borde izquierdo del mapa al que puede acceder el enemigo.

    [SerializeField]
    public Transform rightEdge; // Indica la zona máxima del borde derecho del mapa al que puede acceder el enemigo.

    private Canvas healthCavas; // El canvas de la vida del enemigo.

    //private bool dropItem = true;

    // Indica si el enemigo está en rango de ataque melee.
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;

            }

            return false;
        }
    }

    // Indica si el enemigo está en rango de lanzamiento.
    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;

            }

            return false;
        }
    }

    // Indica si el enemigo está muerto.
    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentValue <= 0;
        }
    }

    // Use this for initialization
    public override void Start()
    {   // Llama a la base start.
        base.Start();

        this.startPos = transform.position;

        // Hace que la función RemoveTarget esté pediente del DeadEvent.
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        // Situa al enemigo en posición idle.
        ChangeState(new IdleState());

        healthCavas = transform.GetComponentInChildren<Canvas>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!IsDead) // Si el enemigo está vivo.
        {
            if (!TakingDamage) // Y no estamos recibiendo daño.
            {
                // Ejecuta el currentState (estado actual) permitiendo que el enemigo empiece a moverse o a atacar.
                currentState.Execute();
            }

            // Hace que el enemigo mire al target.
            LookAtTarget();
        }

    }

    // Elimina el target del enemigo.
    public void RemoveTarget()
    {
        // Elimina el target.
        Target = null;

        // Cambia el estado a patrulla.
        ChangeState(new PatrolState());
    }

    // Hace que el enemigo mire al target.
    private void LookAtTarget()
    {
        // Si hay un target.
        if (Target != null)
        {
            // Calcula la dirección.
            float xDir = Target.transform.position.x - transform.position.x;

            // Si está mirando en la dirección equivocada.
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                // Mira en la otra dirección.
                ChangeDirection();
            }
        }
    }

    // Cambia el estado del enemigo.
    public void ChangeState(IEnemyState newState)
    {
        // Si ya tiene un estado actual.
        if (currentState != null)
        {
            // Llama a la función de salida del estado.
            currentState.Exit();
        }

        // Establece el estado actual como el nuevo estado.
        currentState = newState;

        // Llama a la función de entrada del estado actual.
        currentState.Enter(this);
    }

    // Mueve al enemigo.
    public void Move()
    {
        // Si el enemigo no está atacando.
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                // Establece la velocidad a 1 para reproducir la animación de movimiento.
                MyAnimator.SetFloat("speed", 1);

                // Mueve al enemigo en la dirección correcta.
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState) // Si se encuentra en estado de patrulla cambia su dirreción.
            {
                ChangeDirection();
            }
            else if (currentState is RangedState) // Si se encuentra en estado de rango elimina el target y cambia a estado idle.
            {
                Target = null;
                ChangeState(new IdleState());
            }

        }

    }

    // Obtiene la dirección actual.
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    // Si el enemigo colisiona con un objeto.
    public override void OnTriggerEnter2D(Collider2D other)
    {
        // Llama a la base OnTriggerEnter.
        base.OnTriggerEnter2D(other);

        // Llama a OnTriggerEnter en el estado actual.
        currentState.OnTriggerEnter(other);
    }

    // Hace que el enemigo reciba daño.
    public override IEnumerator TakeDamage()
    {
        if (!healthCavas.isActiveAndEnabled)
        {
            healthCavas.enabled = true;
        }

        //Reduces the health
        healthStat.CurrentValue -= 10;

        if (!IsDead) // Si el enemigo no está muerto ejecuta la animación de recibir daño.
        {
            MyAnimator.SetTrigger("damage");
        }
        else // Si el enemigo está muerto, se asegura de ejecutar la animación de muerte.
        {
            //if (dropItem)
            //{
            //    GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 5), Quaternion.identity);
            //    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), coin.GetComponent<Collider2D>());
            //    dropItem = false;
            //}
   
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }

    // Elimina al enemigo del juego.
    public override void Death()
    {
        //dropItem = true;
        MyAnimator.ResetTrigger("die");
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentValue = healthStat.MaxVal;
        transform.position = startPos;
        healthCavas.enabled = false;
    }

    // Cambia la dirección del enemigo.
    public override void ChangeDirection()
    {
        Transform tmp = transform.Find("EnemyHealthBarCanvas").transform;
        Vector3 pos = tmp.position;
        tmp.SetParent(null);

        base.ChangeDirection();

        tmp.SetParent(transform);
        tmp.position = pos;
    }
}
