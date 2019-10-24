using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

    // Declara al enemigo.
    [SerializeField]
    private Enemy enemy;

    // Si detecta al player, cambiará su target a Player.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.Target = other.gameObject;
        }
    }


    // Si deja de detecta al player, cambiará su target a nulo.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.Target = null;
        }
    }
}
