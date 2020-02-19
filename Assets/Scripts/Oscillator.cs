using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movement;
    [SerializeField] float period;

    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        DoOscillation();
    }

    void DoOscillation()
    {
        if (period <= float.Epsilon) { return; }

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float sine = Mathf.Sin(cycles * tau);

        Vector3 offset = movement * (sine / 2f + 0.5f);
        transform.position = startingPosition + offset;
    }
}
