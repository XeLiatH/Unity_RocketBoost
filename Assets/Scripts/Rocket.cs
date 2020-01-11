using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidbody;
    AudioSource _audio;


    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody>();
        this._audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            this._rigidbody.AddRelativeForce(new Vector3(0f, 10f, 0f));
            if (!this._audio.isPlaying)
            {
                this._audio.Play();
            }
        }
        else
        {
            this._audio.Stop();
        }
    }

    private void Rotate()
    {
        this._rigidbody.freezeRotation = true; // take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(-Vector3.forward);
        }
    }
}
