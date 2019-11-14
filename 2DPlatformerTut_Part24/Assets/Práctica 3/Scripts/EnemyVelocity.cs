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
        public float tiempoRest = 200;
        public float distanciaCentro = 900;
        public double enemyVelocity;
        void Update()
        {
            distanciaCentro = Vector3.Distance(centro.position, playerPosition.position);
            if (tiempoRest > 0)
            {
                tiempoRest -= Time.deltaTime;
            }

            // Creación del sistema
            WriteLine("Gestión de la velocidad de los enemigos", true);
            FuzzySystem system = new FuzzySystem("Gestión de la velocidad de los enemigos");

            WriteLine("1) Agregar las variables", true);

            // Agregar la variable lingüística "Proximidad" (de 0 a 95 m)
            WriteLine("Agregar la variable Proximidad");
            LinguisticVariable proximidad = new LinguisticVariable("Proximidad", 0, 95);
            proximidad.AddValue(new LinguisticValue("Cerca", new LeftFuzzySet(0, 95, 0, 30)));
            proximidad.AddValue(new LinguisticValue("Media", new TrapezoidalFuzzySet(0, 95, 0, 30, 20, 50)));
            proximidad.AddValue(new LinguisticValue("Lejos", new RightFuzzySet(0, 95, 40, 65)));
            system.addInputVariable(proximidad);

            // Agregar la variable lingüística "Tiempo" (de 0 a 200 s)
            WriteLine("Agregar la variable Tiempo");
            LinguisticVariable tiempo = new LinguisticVariable("Tiempo", 0, 200);
            tiempo.AddValue(new LinguisticValue("Muy Poco", new LeftFuzzySet(0, 200, 40, 60)));
            tiempo.AddValue(new LinguisticValue("Poco", new TrapezoidalFuzzySet(0, 200, 40, 60, 90, 110)));
            tiempo.AddValue(new LinguisticValue("Mitad", new TrapezoidalFuzzySet(0, 200, 90, 110, 140, 160)));
            tiempo.AddValue(new LinguisticValue("Mucho", new RightFuzzySet(0, 200, 140, 160)));
            system.addInputVariable(tiempo);

            // Agregar la variable lingüística "Velocidad" (de 500 a 5000)
            WriteLine("Agregar la variable Velocidad");
            LinguisticVariable velocidad = new LinguisticVariable("Velocidad", 500, 5000);
            velocidad.AddValue(new LinguisticValue("Lenta", new LeftFuzzySet(500, 5000, 1000, 2000)));
            velocidad.AddValue(new LinguisticValue("Normal", new TrapezoidalFuzzySet(500, 5000, 1000, 2000, 3000, 4000)));
            velocidad.AddValue(new LinguisticValue("Rápida", new RightFuzzySet(500, 5000, 3000, 4000)));
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
            WriteLine("Caso 1:", true);
            WriteLine("T = 30 (Muy Poco)");
            WriteLine("P = 800 (Cerca)");
            system.SetInputVariable(tiempo, tiempoRest);
            system.SetInputVariable(proximidad, distanciaCentro);
            WriteLine("Resultado: " + system.Solve() + "\n");
            enemyVelocity = system.Solve();
            system.ResetCase();

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