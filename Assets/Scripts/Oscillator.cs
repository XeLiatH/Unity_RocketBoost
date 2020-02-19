using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movement;

    // todo: remove from inspector later
    [SerializeField] [Range(0, 1)] float movementFactor; // 0 for 

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = movement * movementFactor;
        transform.position = this.startingPosition + offset;
    }
}
