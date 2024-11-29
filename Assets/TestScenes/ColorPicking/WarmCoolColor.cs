using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
using UnityEngine.UI;

public class WarmCoolColor : MonoBehaviour
{
    private const double MinimumError = 0.01;
    private static NeuralNet net;
    private static List<DataSet> dataSets;

    public Image I1;
    public Image I2;
    public Image I3;

    public GameObject pointer1;
    public GameObject pointer2;
    public GameObject pointer3;

    bool trained;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        net = new NeuralNet(3, 4, 1);
        dataSets = new List<DataSet>();
        Next();
    }

    void Next()
    {
        Color c = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        I1.color = c;
        I2.color = c;
        I3.color = c;
        double[] C = {(double)I1.color.r, (double)I1.color.g, (double)I1.color.b};

        if (trained)
        {
            double d = tryValues(C);

            if (d > 0.6)
            {
                pointer1.SetActive(true);
                pointer2.SetActive(false);
                pointer3.SetActive(false);
            }
            else if (d < 0.4)
            {
                pointer1.SetActive(false);
                pointer2.SetActive(true);
                pointer3.SetActive(false);
            }
            else
            {
                pointer1.SetActive(false);
                pointer2.SetActive(false);
                pointer3.SetActive(true);
            }
        }
    }

    public void Train(float val)
    {
        double[] C = { (double)I1.color.r, (double)I1.color.g, (double)I1.color.b };
        double[] v = {(double)val};

        dataSets.Add(new DataSet(C, v));

        i++;
        if (!trained && i % 15 == 14) 
            Train(); 
        Next();
    }

    private void Train()
    {
        net.Train(dataSets, MinimumError);
        trained = true;
    }

    double tryValues(double[] vals)
    {
        double[] result = net.Compute(vals);
        return result[0];
    }

    // Update is called once per frame
    /* void Update()
    {
        
    }*/
}
