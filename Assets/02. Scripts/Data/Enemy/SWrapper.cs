using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SWrapper<T>
{
    public List<T> items;

    public SWrapper(List<T> items)
    {
        this.items = items;
    }
}
