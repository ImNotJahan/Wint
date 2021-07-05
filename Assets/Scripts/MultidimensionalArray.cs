using System;
using UnityEngine;

public class MultidimensionalArray : ScriptableObject
{
    [Serializable]
    public class Arr
    {
        public string[] arr = new string[] { };

        public string this[int i]
        {
            get
            {
                return arr[i];
            }
            set
            {
                arr[i] = value;
            }
        }

        public int Length()
        {
            return arr.Length;
        }
    }
}
