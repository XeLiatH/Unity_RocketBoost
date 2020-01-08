using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // print("Speeding up");
            this._rigidbody.AddRelativeForce(new Vector3(0f, 10f, 0f));
        }

        if (Input.GetKey(KeyCode.A))
        {
            // print("Rotating left");
            this.transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // print("Rotating right");
            this.transform.Rotate(-Vector3.forward);
        }
    }
}
