using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; //Siempre que queramos programar algo con A* debemos incluirlo

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath; //Nombre del script que controla al enemigo

    // Update is called once per frame
    void Update()
    {
       if(aiPath.desiredVelocity.x >= 0.01f) //Velocidad a la que el enemigo querrá desplazarse según la ruta que tenga que tomar para alcanzar al jugador
        {
            transform.localScale = new Vector3(1f, 1f, 1f); //Si el valor es mayor que 0.01 mirará a la derecha
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); //Si el valor es menor que 0.01 mirará a la izquierda
        }
    }
}
