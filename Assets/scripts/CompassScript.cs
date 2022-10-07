using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour
{

    public Transform n;
    public Transform w;
    public Transform e;
    public Transform s;

    public Transform GetNorth()
    {
        return n;
    }

    public Transform GetWest()
    {
        return w;
    }

    public Transform GetEast()
    {
        return e;
    }

    public Transform GetSout()
    {
        return s;
    }
  
}
