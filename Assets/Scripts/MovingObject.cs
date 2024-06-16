using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] 
    private Vector3 distanceToMove;
    [SerializeField] 
    private float velocityFactor = 1;
    
    [SerializeField]
    private Vector3 rotationSpeed;
    private Vector3 startingPoint;
    private Vector3 destinationPoint;
    private bool increaseValue = true;
    
    private float passedTimeForInterpolation = 0;

    void Start()
    {
        startingPoint = transform.position;
        destinationPoint = startingPoint + distanceToMove;
    }

    void Update()
    {
        if (increaseValue)
            passedTimeForInterpolation += Time.deltaTime * velocityFactor;
        else
            passedTimeForInterpolation -= Time.deltaTime * velocityFactor;
        
        if (passedTimeForInterpolation > 1)
            increaseValue = false;
        else if (passedTimeForInterpolation < 0)
            increaseValue = true;
        
        Vector3 result = Vector3.Lerp(startingPoint, destinationPoint, passedTimeForInterpolation);
        transform.position = result;

        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
