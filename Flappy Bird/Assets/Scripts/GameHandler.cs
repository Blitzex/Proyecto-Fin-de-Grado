using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usa las herramientas de CodeMonkey.
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    private void Start() {
        Debug.Log("GameHandler.Start");


        //Ejecuta la función cada 300 milisegundos indicando en el lugar donde se encuentra el ratón la palabra "Ding" más un contador con el número de veces.
        /*
           int count = 0;
           FunctionPeriodic.Create(() => {
               CMDebug.TextPopupMouse("Ding! " + count);
               count++;
               }, .300f);

           */

        //Para referenciar Sprites.

        //GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHeadSprite;
}
}
