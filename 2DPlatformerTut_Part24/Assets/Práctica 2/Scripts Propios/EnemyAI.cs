using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;  //Referencia al target

    public float speed = 200f; //Velocidad del enemigo
    public float nextWaypointDistance = 3f; //Distancia a la que tiene que encontrarse el enemigo antes de ir al próximo waypoint
    public Transform enemyGFX; 

    Path path; //El path que está siguiendo
    int currentWaypoint = 0; //El waypoint del path que está siguiendo
    bool reachedEndOfPath = false; //Indica cuando ha alcanzado el final del path

    Seeker seeker; //Referencia al script "Seeker"
    Rigidbody2D rb; //Referencia al rigidbody 2d

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>(); //Obtiene el valor del seeker
        rb = GetComponent<Rigidbody2D>(); //Obtiene el valor del rigidbody 2d

        InvokeRepeating("UpdatePath", 0f, .5f); //Llama a la función "UpdatePath" cada medio segundo, de modo que vuelva a calcular el path
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone()) //Si el seeker no está calculando ningún path...
            seeker.StartPath(rb.position, target.position, OnPathComplete); //Empieza el path por donde se encuentra situado y termina cuando alcanza al target
    }

    void OnPathComplete(Path p) //Función a la que se llama cuando se completa el path
    {
        if (!p.error) //Si no se obtiene ningún error
        {
            path = p; //El path pasa a ser igual al generado
            currentWaypoint = 0; //Establece el waypoint actual en 0, de modo que será el comienzo del nuevo path
        }
    }

    // Update is called once per frame
    void FixedUpdate() //Es un update que solo es llamado un numero concreto de veces por segundo, perfecto para trabajar con físicas
    {
        if (path == null) //Si no hay path sale de la función
            return;

        if (currentWaypoint >= path.vectorPath.Count) //Si el total de waypoints del path es mayor o igual que el actual...
        {
            reachedEndOfPath = true; //Hemos alcanzado el final del path
            return;
        }
        else
        {
            reachedEndOfPath = false; //Si no es mayor o igual, siguen quedando waypoints que recorrer
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //Obtiene la dirección del siguiente waypoint a alcanzar
        Vector2 force = direction * speed * Time.deltaTime; //La fuerza que será aplicada al enemigo para que se mueva en esa dirección

        rb.AddForce(force); //Añade fuerza al enemigo

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //Distancia al próximo waypoint

        if(distance < nextWaypointDistance) //Si esa distancia es menor que la distancia al próximo waypoint...
        {
            currentWaypoint++; //Hemos alcanzado el waypoint y nos movemos al siguiente
        }

        if (force.x >= 0.01f) //Velocidad a la que el enemigo querrá desplazarse según la ruta que tenga que tomar para alcanzar al jugador
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f); //Si el valor es mayor que 0.01 mirará a la derecha
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f); //Si el valor es menor que 0.01 mirará a la izquierda
        }

    }
}
