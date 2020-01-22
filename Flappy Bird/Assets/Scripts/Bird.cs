using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;


//Hace que el pájaro salte cada vez que utilizamos el clic. Utiliza las físicas de Unity.



public class Bird : MonoBehaviour {

    private const float JUMP_AMOUNT = 90f; //Indica cuanto va a saltar el pájaro cuando usemos el clic o el espacio.

    private static Bird instance;

    public static Bird GetInstance() {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdRigidbody2D;
    public State state;

    public enum State {
        WaitingToStart,
        Playing,
        Dead
    }

    public bool muerto;

    //Hace referencia al RigidBody2D de Unity para el pájaro.

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;



        birdRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        //gameObject.GetComponent<Rigidbody2D>();
        //sr.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        GetComponent<SpriteRenderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

    }


    private void Update() {
        switch (state) {
        default:
        case State.WaitingToStart:
            if (TestInput()) {
                // Start playing
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
            }
            break;
        case State.Playing:
            if (TestInput()) {
                Jump();
            }

            // Rota el personaje cuando salta. 
            transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * .15f);
                muerto = false;
            break;
        case State.Dead:
                muerto = true;
               // fitness = ControllerGame.I.time;
            break;
        }
    }


    void Flap()
    {
        if (!muerto)
        {
            birdRigidbody2D.velocity = new Vector2(0, JUMP_AMOUNT);
        }
    }

    private bool TestInput() {
        return 
            Input.GetKeyDown(KeyCode.Space) || //ejecuta el salto cuando pulsamos espacio.
            Input.GetMouseButtonDown(0) || //ejecuta el salto cuando pulsamos clic izquierdo.
            Input.touchCount > 0; //ejecuta el salto cuando pulsamos la pantalla.
    }

    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT; // Hace que el salto del pájaro sea constante y del mismo tamaño.
        SoundManager.PlaySound(SoundManager.Sound.BirdJump); // Hace que suene un sonido al saltar.
    }

    private void OnTriggerEnter2D(Collider2D collider) {  //Hace que cuando el pájaro choque con una tubería muera, suena el sonido de perder la partida, 
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

}
