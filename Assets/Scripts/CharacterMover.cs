using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMover : MonoBehaviour
{
    // Allows us to hookup calls to when the final destination is reached
    public UnityEvent OnFinalDestinationReached;

    // These can be set in the inspector in the Unity editor
    // For this sample I've used the DoorTarget object inside each Place object
    public List<Transform> Destinations;

    public DefaultTrackableEventHandler ScenarioImageTracker;

    // When the character sees this image it will be "scared"
    public DefaultTrackableEventHandler ScaredOfThisImageTracker;

    public float MoveSpeed = 0.05f;
    public float ScaredMoveSpeed = 0.01f;

    private int CurrentDestinationIndex = 0;
    private bool DoMovement = false;
    private bool IsScared = false;
    private Transform MyTransform;
    private float step;
    private Vector3 pointOnPlane;

    // Start is called before the first frame update
    void Start()
    {
        MyTransform = transform;
        
        // Will break of this has not been set up in the editor
        ScenarioImageTracker.OnTargetFound.AddListener(TurnMoveToDestinationsOn);
        ScenarioImageTracker.OnTargetLost.AddListener(TurnMoveToDestinationsOff);

        ScaredOfThisImageTracker.OnTargetFound.AddListener(TurnScaredOn);
        ScaredOfThisImageTracker.OnTargetLost.AddListener(TurnScaredOff);
    }

    // Update is called once per frame
    void Update()
    {
        if (DoMovement && CurrentDestinationIndex < Destinations.Count) {
            // Based on Unity documenation: https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
            float currentMoveSpeed = MoveSpeed;

            if (IsScared) {
                currentMoveSpeed = ScaredMoveSpeed;
            }
            step = currentMoveSpeed * Time.deltaTime;

            var target = Destinations[CurrentDestinationIndex];

            // Moves us a step closer to the object. 
            // We will rely on collision triggers to determine when the object has been reached instead of checking
            // the positions here
            MyTransform.position = Vector3.MoveTowards(MyTransform.position, target.position, step);

            // This rotation code from http://answers.unity.com/answers/867743/view.html
            float distanceToPlane = Vector3.Dot(MyTransform.up, target.position - MyTransform.position);
            pointOnPlane = target.position - (MyTransform.up * distanceToPlane);

            MyTransform.LookAt(pointOnPlane, MyTransform.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered:" + other.name);

        if (Destinations[CurrentDestinationIndex].gameObject == other.gameObject)
        {
            // There are probably cleaner and less coupled ways of doing this
            var DoorTarget = other.gameObject.GetComponent<DoorTarget>();
            if (DoorTarget != null)
            {
                Debug.Log("Door hit");
                DoorTarget.CloseDoor();
            }
            CurrentDestinationIndex++;

            if (CurrentDestinationIndex >= Destinations.Count)
            {
                TurnMoveToDestinationsOff();
                if (OnFinalDestinationReached != null)
                {
                    OnFinalDestinationReached.Invoke();
                    if (ZombieAttack.touching == true) {
                        var target = Destinations[1];
                        MyTransform.position = Vector3.MoveTowards(MyTransform.position, target.position, step);
                        MyTransform.LookAt(pointOnPlane, MyTransform.up);
                        TurnMoveToDestinationsOn();
                        TurnScaredOn();
                    }
                }

                MyTransform.localPosition = Vector3.zero;
            }
        }
    }

    public void TurnMoveToDestinationsOn() {
        DoMovement = true;
    }

    public void TurnMoveToDestinationsOff() {
        DoMovement = false;
    }

    private void TurnScaredOff() {
        IsScared = false;
    }

    private void TurnScaredOn() {
        IsScared = true;
    }
}
