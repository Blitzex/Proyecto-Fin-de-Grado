using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player4 : MonoBehaviour
{
    public float speed = 4f; //La velocidad del player

    void Start ()
    {

    }

    void Update ()
    {
        //Permite al player desplazarse por el mapa en horizontal y en vertical.
        Vector3 mov = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + mov, speed * Time.deltaTime);

    }
}
