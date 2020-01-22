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
    private State state;

    private enum State {
        WaitingToStart,
        Playing,
        Dead
    }

    //Hace referencia al RigidBody2D de Unity para el pájaro.

    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
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
            break;
        case State.Dead:
            break;
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
