using System.Collections.Generic;
using UnityEngine;

/// <summary>Used to shuffle collections.</summary>

public class Shuffler
{
    /// <summary>Shuffles the specified array.</summary>

    public void Shuffle<T>(IList<T> array)
    {
        for (int n = array.Count; n > 1;)
        {
            int k = Random.Range(0, n);
            --n;
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    /// <summary>Shuffles the specified 2D array.</summary>

    public void Shuffle<T>(T[,] array)
    {
        int w = array.GetUpperBound(1) + 1;

        for (int n = array.Length; n > 1;)
        {
            int k = Random.Range(0, n);
            --n;

            int dr = n / w;
            int dc = n % w;
            int sr = k / w;
            int sc = k % w;

            T temp = array[dr, dc];
            array[dr, dc] = array[sr, sc];
            array[sr, sc] = temp;
        }
    }
}