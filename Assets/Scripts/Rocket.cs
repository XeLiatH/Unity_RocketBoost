using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 20f;
    [SerializeField] float rotationSpeed = 100f;

    Rigidbody _rigidbody;
    AudioSource _audio;

    int _currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody>();
        this._audio = GetComponent<AudioSource>();
        this._currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                print("good");
                break;
            case "Finish":
                print("finish");
                SceneManager.LoadScene(this._currentSceneIndex + 1);
                break;
            default:
                print("bad");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            this._rigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
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

        Vector3 rotationThisFrame = Vector3.forward * rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(-rotationThisFrame);
        }
    }
}
