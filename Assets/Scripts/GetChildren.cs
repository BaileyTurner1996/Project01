using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChildren : MonoBehaviour
{
    public float counter;
    void Start()
    {
        List<Transform> children = GetChild(transform, true);
        counter = (children.Count / 3);
        //Debug.Log(counter);
        
    }

    private void Update()
    {
        List<Transform> children = GetChild(transform, true);
        counter = (children.Count / 3);
    }


    List<Transform> GetChild(Transform parent, bool recursive)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent) 
        {
            children.Add(child);
            if (recursive)
            {
                children.AddRange(GetChild(child, true));
            }
        }
        return children;
    }
}
