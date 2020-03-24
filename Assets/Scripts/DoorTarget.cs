using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTarget : MonoBehaviour
{
    // This lets us create event hookups in the inspector
    public UnityEvent OnDoorClosed;

    public void CloseDoor()
    {
        Debug.Log("Close Door");
        if (OnDoorClosed != null)
        {
            Debug.Log("Close Door call hooked up inspector methods");
            // In this sample project we will hook it to the Place object so that it can be destroyed
            // A component on that object is ShrinkAwayAndDestroy so I chose
            // ShrinkAwayAndDestroy.StartShrinking
            // This is all hooked up through the inspector. The Invoke funtion will call
            // all the hooked up methods in the inspector
            OnDoorClosed.Invoke();
        }
    }
}
