using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAwayAndDeactivate : MonoBehaviour
{
    public float ScaleSpeed = 1.0f;
    
    // Will scale until the object becomes less than 10% of its starting scale (in X axis as an arbitrary choice)
    private float MinScalePercentage = 0.1f;
    private float StartingScale;

    private bool DoShrink = false;
    private bool ShrinkComplete = false;

    private Transform MyTransform;

    void Awake()
    {
        MyTransform = transform;
    }

    void Start()
    {
        // Awake is called before start so we should know that the transform is available with values set
        StartingScale = MyTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoShrink && !ShrinkComplete)
        {
            // Scale down over time
            float scaleBy = ScaleSpeed * Time.deltaTime;
            var newScale = new Vector3(MyTransform.localScale.x - scaleBy,
                MyTransform.localScale.y - scaleBy,
                MyTransform.localScale.z - scaleBy);

            MyTransform.localScale = newScale;

            if ((newScale.x / StartingScale) <= MinScalePercentage)
            {
                ShrinkComplete = true;
                DoShrink = false;

                gameObject.SetActive(false);
            }
        }
    }

    public void StartShrinking()
    {
        Debug.Log("Start shrinking:" + name);
        DoShrink = true;
    }
}
