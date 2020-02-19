using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movement;
    [SerializeField] float period;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // todo: protect against period being 0
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float sine = Mathf.Sin(cycles * tau);

        Vector3 offset = movement * (sine / 2f + 0.5f);
        transform.position = this.startingPosition + offset;
    }
}
