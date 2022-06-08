using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN
{
    public const float mutationChance = 0.05f;
    public Layer[] layers;
    public int[] sizes;
    const float minWeight = -1f;
    const float maxWeight = 1f;
    const float minBias = -1f;
    const float maxBias = 1f;
    public NN (int[] sizes)
    {
        this.sizes = sizes;
        layers = new Layer[sizes.Length];
        for (int i = 0; i < sizes.Length; i++)
        {
            int nextSize = 0;
            if (i < sizes.Length - 1) nextSize = sizes[i + 1];
            layers[i] = new Layer(sizes[i], nextSize);
            for (int j = 0; j < sizes[i]; j++)
            {
                if(i != 0 && i != sizes.Length)
                {
                    layers[i].bias[j] = UnityEngine.Random.Range(minBias, maxBias);
                }
                else
                {
                    layers[i].bias[j] = 0f;
                }
                for (int k = 0; k < nextSize; k++)
                {
                    layers[i].weights[j, k] = UnityEngine.Random.Range(minWeight, maxWeight);
                }
            }
        }
    }
    void Mutate()
    {
        for (int i = 0; i < sizes.Length; i++)
        {
            int nextSize = 0;
            if (i < sizes.Length - 1) nextSize = sizes[i + 1];
            for (int j = 0; j < sizes[i]; j++)
                for (int k = 0; k < nextSize; k++)
                    if (UnityEngine.Random.value < mutationChance)
                        layers[i].weights[j, k] = UnityEngine.Random.Range(minWeight, maxWeight);
        }
    }
    public NN(Layer[] layers, int[] sizes)
    {
        this.sizes = sizes;
        this.layers = layers;
        Mutate();
    }
    public float[] FeedForward(float[] inputs)
    {
        Array.Copy(inputs, 0, layers[0].neurons, 0, inputs.Length);
        for (int i = 1; i < layers.Length; i++)
        {
            Layer l = layers[i - 1];
            Layer l_next = layers[i];
            for (int j = 0; j < l_next.size; j++)
            {
                l_next.neurons[j] = l_next.bias[j];
                for (int k = 0; k < l.size; k++)
                {
                    l_next.neurons[j] += l.neurons[k] * l.weights[k, j];
                }
            }
            for (int j = 0; j < layers[i].neurons.Length; j++)
            {
                layers[i].neurons[j] = Activate(layers[i].neurons[j]);
            }
        }
        return layers[layers.Length - 1].neurons;
    }
    float Activate(float x)
    {
        return 2 / (1 + Mathf.Exp(-2 * x)) - 1;
    }
}
