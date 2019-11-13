using FuzzyLogicPCL;
using FuzzyLogicPCL.FuzzySets;
using System;
using UnityEngine;

namespace FuzzyLogicApp
{
    public class EnemyVelocity: MonoBehaviour
    {
        public Transform centro;
        public Transform playerPosition;
        float tiempoRest = 200;
        float distanciaCentro = 900;
        public double enemyVelocity;
        void Update()
        {
            distanciaCentro = Vector3.Distance(centro.position, playerPosition.position);
            if (tiempoRest > 0)
            {
                tiempoRest -= Time.deltaTime;
            }
            Debug.Log(tiempoRest);

            // Creación del sistema
            WriteLine("Gestión de la velocidad de los enemigos", true);
            FuzzySystem system = new FuzzySystem("Gestión de la velocidad de los enemigos");

            WriteLine("1) Agregar las variables", true);

            // Agregar la variable lingüística "Proximidad" (de 0 a 900 m)
            WriteLine("Agregar la variable Proximidad");
            LinguisticVariable proximidad = new LinguisticVariable("Proximidad", 0, 900);
            proximidad.AddValue(new LinguisticValue("Lejos", new LeftFuzzySet(0, 900, 300, 400)));
            proximidad.AddValue(new LinguisticValue("Media", new TrapezoidalFuzzySet(0, 900, 300, 400, 600, 700)));
            proximidad.AddValue(new LinguisticValue("Cerca", new RightFuzzySet(0, 900, 600, 700)));
            system.addInputVariable(proximidad);

            // Agregar la variable lingüística "Tiempo" (de 0 a 200 s)
            WriteLine("Agregar la variable Tiempo");
            LinguisticVariable tiempo = new LinguisticVariable("Tiempo", 0, 200);
            tiempo.AddValue(new LinguisticValue("Muy Poco", new LeftFuzzySet(0, 200, 40, 60)));
            tiempo.AddValue(new LinguisticValue("Poco", new TrapezoidalFuzzySet(0, 200, 40, 60, 90, 110)));
            tiempo.AddValue(new LinguisticValue("Mitad", new TrapezoidalFuzzySet(0, 200, 90, 110, 140, 160)));
            tiempo.AddValue(new LinguisticValue("Mucho", new RightFuzzySet(0, 200, 140, 160)));
            system.addInputVariable(tiempo);

            // Agregar la variable lingüística "Velocidad" (de 1 a 5)
            WriteLine("Agregar la variable Velocidad");
            LinguisticVariable velocidad = new LinguisticVariable("Velocidad", 0, 5);
            velocidad.AddValue(new LinguisticValue("Lenta", new LeftFuzzySet(0, 5, 1, 2)));
            velocidad.AddValue(new LinguisticValue("Normal", new TrapezoidalFuzzySet(0, 5, 1, 2, 3, 4)));
            velocidad.AddValue(new LinguisticValue("Rápida", new RightFuzzySet(0, 5, 3, 4)));
            system.addOutputVariable(velocidad);

            WriteLine("2) Agregar las reglas", true);

            // Creación de las reglas en función de la matriz siguiente:
            // Cuanto mayor sea la Proximidad y menor sea el Tiempo, más Velocidad tendrán.
            // P \ T    || L | M | C |
            // Muy Poco || N | R | R |
            // Poco     || N | N | R |
            // Mitad    || L | N | N |
            // Mucho    || L | L | N |
            system.addFuzzyRule("IF Proximidad IS Lejos AND Tiempo IS Muy Poco THEN Velocidad IS Normal");
            system.addFuzzyRule("IF Proximidad IS Lejos AND Tiempo IS Poco THEN Velocidad IS Normal");
            system.addFuzzyRule("IF Proximidad IS Lejos AND Tiempo IS Mitad THEN Velocidad IS Lenta");
            system.addFuzzyRule("IF Proximidad IS Lejos AND Tiempo IS Mucho THEN Velocidad IS Lenta");
            system.addFuzzyRule("IF Proximidad IS Media AND Tiempo IS Muy Poco THEN Velocidad IS Rápida");
            system.addFuzzyRule("IF Proximidad IS Media AND Tiempo IS Poco THEN Velocidad IS Normal");
            system.addFuzzyRule("IF Proximidad IS Media AND Tiempo IS Mitad THEN Velocidad IS Normal");
            system.addFuzzyRule("IF Proximidad IS Media AND Tiempo IS Mucho THEN Velocidad IS Lenta");
            system.addFuzzyRule("IF Proximidad IS Cerca AND Tiempo IS Muy Poco THEN Velocidad IS Rápida");
            system.addFuzzyRule("IF Proximidad IS Cerca AND Tiempo IS Poco THEN Velocidad IS Rápida");
            system.addFuzzyRule("IF Proximidad IS Cerca AND Tiempo IS Mitad THEN Velocidad IS Normal");
            system.addFuzzyRule("IF Proximidad IS Cerca AND Tiempo IS Mucho THEN Velocidad IS Normal");
            WriteLine("12 reglas agregadas \n");

            WriteLine("3) Resolución de casos prácticos", true);
            
            system.ResetCase();
            WriteLine("Caso 1:", true);
            WriteLine("T = 30 (Muy Poco)");
            WriteLine("P = 800 (Cerca)");
            system.SetInputVariable(tiempo, tiempoRest);
            system.SetInputVariable(proximidad, distanciaCentro);
            WriteLine("Resultado: " + system.Solve() + "\n");
            enemyVelocity = system.Solve();

            //// Caso práctico 2: Tiempo de 180s, proximidad a la meta de 280m
            //system.ResetCase();
            //WriteLine("Caso 2:", true);
            //WriteLine("T = 180 (Mucho)");
            //WriteLine("P = 280 (Lejos)");
            //system.SetInputVariable(tiempo, 180);
            //system.SetInputVariable(proximidad, 280);
            //WriteLine("Resultado: " + system.Solve() + "\n");

            //// Caso práctico 3: Tiempo de 70s, proximidad a la meta de 530m
            //system.ResetCase();
            //WriteLine("Caso 3:", true);
            //WriteLine("T = 70 (Poco)");
            //WriteLine("P = 530 (Media)");
            //system.SetInputVariable(tiempo, 70);
            //system.SetInputVariable(proximidad, 530);
            //WriteLine("Resultado: " + system.Solve() + "\n");

            //// Caso práctico 4: Tiempo de 120s, proximidad a la meta de 120m
            //system.ResetCase();
            //WriteLine("Caso 4:", true);
            //WriteLine("T = 120 (Mitad)");
            //WriteLine("P = 120 (Lejos)");
            //system.SetInputVariable(tiempo, 120);
            //system.SetInputVariable(proximidad, 120);
            //WriteLine("Resultado: " + system.Solve() + "\n");

            //// Caso práctico 5 : Tiempo de 10s, proximidad a la meta de 450m
            //system.ResetCase();
            //WriteLine("Caso 5:", true);
            //WriteLine("T = 10 (Muy Poco)");
            //WriteLine("P = 450 (Media)");
            //system.SetInputVariable(tiempo, 10);
            //system.SetInputVariable(proximidad, 450);
            //WriteLine("Resultado: " + system.Solve() + "\n");
        }
        /// <summary>
        /// Ayuda para escribir mensajes por consola (agregando *)
        /// </summary>
        /// <param name="msg">Mensaje a mostrar</param>
        /// <param name="stars">¿Necesita asteriscos?</param>
        private static void WriteLine(string msg, bool stars = false)
        {
            if (stars)
            {
                msg = "*** " + msg + " ";
                while (msg.Length < 45)
                {
                    msg += "*";
                }
            }
            Console.WriteLine(msg);
        }
    }
}