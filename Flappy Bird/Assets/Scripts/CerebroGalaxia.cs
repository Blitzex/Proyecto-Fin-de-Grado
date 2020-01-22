using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class CerebroGalaxia : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("Controlador").GetComponent<Controlador>();
        for (int i = 0; i < generationSize; i++) {
            GameObject navIns = Instantiate(birdsObj, new Vector2(0, 0), Quaternion.identity);
            birdsScript.Add(navIns.GetComponent<MovementFlappys>());
        }
            elitismoN = (int)(elitism * (float)birdsScript.Count);

 
        }

    public void Reproduce(ref List<MovementFlappys> antepasadas)
    {
        List<MovementFlappys> descedientes = new List<MovementFlappys>();
        for (int i = 0; i < elitismoN; i++)
        {
            descedientes.Add(antepasadas[i]);
        }
        while (descendientes.Count < antepasadas.Count)
        {
            int loteria = Random.Range(generationWorst, generationBest);
            int azar = Random.Range(0, antepasadas.Count);
            while (antepasadas [azar].fitness < loteria)
            {
                azar = Random.Range(0, antepasadas.Count);
            }
            descendiente.Add(antepasadas[azar]);
        }
        for (int i = 0; i< antepasadas.Count; i++)
        {
            antepasadas[i].cerebro.setWeights(descendientes[i].cerebro.getWeights());
        }
    }

    public void Crossover (ref List<MovementFlappys> antepasados)
    {
        List<MovementFlappys> padres = new List<MovementFlappys>();
        List<MovementFlappys> madres = new List<MovementFlappys>();
        List<MovementFlappys> hijos = new List<MovementFlappys>();
        int Ncrossovers = (int)((float)antepasados.Count * crossoverRate);
        int contador = 0;
        if (Ncrossovers % 2 !=0)
        {
            Ncrossovers--;
        }
    }
    while (contador < Ncrossovers) {

        int azar = Random.Range(elistismoN, antepasados.Count);
        int azar2 = Random.Range(0, antepasdos.Count);
        int azar3 = Random.Range(0, antepasdos.Count);
        hijos.Add (antepasados [azar]);
        padres.Add(antepasados[azar2]);
        madres.Add(antepasados[azar3]);
        MovementFlappys hijo = hijos[contador];
        Mating(padres[contador], madres[contador], ref hijo);
        antepasados.RemoveAt(azar);
        contador++;
        antepasados.Insert(contador,hijo);
    }
}

public void Mating (MovementFlappys madre, MovementFlappys padre, ref MovementFlappys hijo)
{
    List<double> genomaMadre = madre.cerebro.getWeights();
    List<double> genomaMadre = padre.cerebro.getWeights();

    int inicio = Random.Range (0, genomaPadre.Count);
    int final = Random.Range(inicio, genomaPadre.Count);

    for (int i = inicio; i < final; i++)
    {
        genomaMadre[i] = genomaPadre[i];
    }
    hijo.cerebro.setWeights(genomaMadre);
}

public void Mutate(ref SistemaNeuronal cerebro)
{
    List<double> genoma = cerebro.getWeights();
    int Nmutaciones = (int)(mutationRate * (float)genoma.Count);
    for (int i = 0; i < NmutationRate; i++)
    {
        int azar = Random.Range(0, genoma.Count);
        float factor = Random.Range(-1f, 1f);
        genoma[azar] += alpha * factor;
    }
    cerebro.setWeights(genoma);
}

void NewGeneration()
{
    ControllerGame.I.time = 0;
    birdsScript = birdsScript.OrderByDescending(order => order.fitness).ToList();
    generationBest = birdsScript[0].fitness;
    generationWorst = birdsScript[birdsScript.Count - 1].fitness;
    if (generationBest > bestFitness)
    {
        bestFitness = generationBest;
    }
    fitnesses.Add(generationBest);

    Reproduce(ref birdsScript);
    Crossover(ref birdsScript);

    for (int i = elitismoN; i < bindsScript.Count; i++)
    {
        Mutate(ref birdsScript[i].cerebro);
    }

    foreach (MovementFlappys bird in birdsScript)
    {
        bird.transform.position = new Vector3(0, 0, 0);
        bird.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        bird.fitness = 0;
        bird.muerto = false;
    }
    generation++;

}

    // Update is called once per frame
    void Update()
    {

    GameObject[] interPipeLines = GameObject.FindGameObjectsWithTag("Finish");
    if (siguiente == null)
    {
        if (interPipeLines.Length > 0)
            siguiente = interPipeLines[0];
    }
    else
    {
        for (int i = 0; i < interPipeLines.Length; i++)
        {
            if (interPipeLines[i].transform.position.x > outVision &&
                (interPipeLines[i].transform.position.x < siguiente.transform.position.x || siguiente.transform.position.x < outVision)) {
                siguiente = interPipeLines[i];
                ControllerGame.I.puntos = muerto ? ControllerGame.I.puntos : ControllerGame.I.puntos + 1;
        }
        }
        nextHeight = siguiente.transform.position.y;
        distanceToNext = siguiente.transform.position.x;
    }
    if (!ControllerGame.I.manual)
    {
        double[] entradas = { (double)Transform.position.y - nextHeight, distanceToNext };

        if (cerebro.Evaluate(entradas) > 0.5f)
        {
            Flap();

        }
    }
    if (Imput.GetKeyDown ("Space"));
    }

    internal NeuralNetwork(int _nbInputs, int _nbHidden, int _nbOutput)
    {
    _nbInputs = _nbInputs;
    _nbHidden = _nbHidden;
    nbOutputs = _nbOutput;

    hiddenNeurons = new Neuron[nbHidden];
    for (int i = 0; i < _nbHidden; i++)
    {
        outputNeurons[i] = new Neuron(_nbHidden);
    }
}

public void NuevaRed()
{
    Red = new NeuralNetwork(2, 4, 1);
    ActualizarPesos();
}

internal double Evaluate(double[] _inputs)
{
    if (output.Equals(double.NaN))
    {
        double x = 0.0;
        for (int i = 0; i < nbInputs; i++)
        {
            x += _inputs[i] * weights[i];
        }
        x += weights[nbInputs];

        output = 1.0 / (1.0 + Mathf.Exp(-1.0 * x));
    }
    return output;
}

internal double[] Evaluate(double[] _inputs)
{
    foreach (Neuron n in hiddenNeurons)
    {
        n.Clear();
    }
    foreach (Neuron n in outputNeurons)
    {
        n.Clear();
    }

    double[] hiddenOutputs = new double[nbHidden];

    for (int i = 0; i < nbHidden; i++)
    {
        hiddenOutputs[i] = hiddenNeurons[i].Evaluate(_inputs);
    }

    double[] outputs = new double[nbOutputs];

    for (int outputNb =0; outputNb < nbOutputs; outputNb++)
    {
        outputs[outputNb] = outputNeurons[outputNb].Evaluate(hiddenOutputs);
    }

    return outputs;
}

   if (cerebro.Evaluate(entradas) > 0.5f)
    {
        Flap();
    }

void OnTriggerEnter2D(Collider2D col)
{
    if ((col.gameObject.tag == "Pipe" || col.gameObject.tag== "Obstacle") && !muerto)
    {
        muerto = true;
        fitness = ControllerGame.I.time;
        List<string> sList = new List<string>();
        sList.Add(siguiente.transform.position.x.ToString() + "\t" + ((double)transform.position.y - siguiente.transform.position.y) + "\t" +
        siguiente.transform.position.x.ToString() + "\t" + ((double)Transform.position.y - siguiente.transform.position.x));
        cerebro.AjustarPesos(sList.ToArray());
    }
}

internal void AdjustWeights(DataPoint _point, double _learningRate)
{
    double[] outputDeltas = new double[nbOutputs];
    for (int i = 0; i < nbOutputs; i++)
    {
        double output = outputNeurons[i].Output;
        double expectedOutput = _point.Outputs[i];
        outputDeltas[i] = output * (1 - output) * (expectedOutput - output);
    }

    double[] hiddenDeltas = new double[nbHidden];
    for (int i = 0; i < nbHidden; i++)
    {
        double hiddenOutput = hiddenNeurons[i].Output;
        double sum = 0.0;
        for (int j = 0; j < nbOutputs; j++)
        {
            sum += outputDeltas[j] * outputNeurons[j].Weight(i);
        }
        hiddenDeltas[i] = hiddenOutput * (1 - hiddenOutput) * sum;
    }
    double value;
    for (int i = 0; i < nbOutputs; i++)
    {
        Neuron outputNeuron = outputNeurons[i];
        for (int j = 0; j < nbHidden; j++)
        {
            value = outputNeuron.Weight(j) + _learningRate * outputDeltas[i] * hiddenNeurons[j].Output;
            outputNeuron.AdjustWeight(j, value);
        }
        value = outputNeuron.Weight(j) + _learningRate * outputDeltas[i] * 1.0 ;
        outputNeuron.AdjustWeight(j, value);
    }
    for (int i = 0; i < nbHidden; i++)
    {
        Neuron hiddenNeuron = hiddenNeurons[i];
        for (int j = 0; j < nbInputs; j++)
        {
            value = hiddenNeuron.Weight(j) + _learningRate * hiddenDeltas[i] * _point.Inputs[j];
            hiddenNeuron.AdjustWeight(j, value);
        }
        value = hiddenNeuron.Weight(nbInputs) + _learningRate * hiddenDeltas[i] * 1.0;
        outputNeuron.AdjustWeight(nbInputs, value);
    }
}



}
