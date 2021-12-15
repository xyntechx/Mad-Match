using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public bool immobile = true;

    public bool set_immobile(bool flag) // returns original value of immobile
    {
        bool orig_immobile = immobile;
        immobile = flag;
        return orig_immobile;
    }
}
