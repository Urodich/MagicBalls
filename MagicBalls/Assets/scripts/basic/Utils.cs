using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool LayerComparer(GameObject gameObject, LayerMask layerMask){
        return 1<<gameObject.layer == (1 << gameObject.layer & layerMask);
    }
}
