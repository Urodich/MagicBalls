using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class start_event_script : MonoBehaviour
{
    [SerializeField] UnityEvent Event;
    void Start()
    {
        Event?.Invoke();
    }


}
