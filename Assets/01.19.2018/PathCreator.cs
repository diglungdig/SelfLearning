using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {

    [HideInInspector]
    public Pathz path;
    
    public void CreatePath()
    {
        path = new Pathz(transform.position);
    }
}
