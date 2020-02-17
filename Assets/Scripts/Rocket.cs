using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 20f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float levelDelay = 1f;
    [SerializeField] float deathDelay = .5f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip successSFX;

    Rigidbody _rigidbody;
    AudioSource _audio;

    int _currentSceneIndex;

    enum State { Alive, Dying, Transcending }
    State _state = State.Alive;

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
        if (this._state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (this._state != State.Alive) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                // do nothing, everything is ok
                break;
            case "Finish":
                StartSuccessProcess();
                break;
            default:
                StartDeathProcess();
                break;
        }
    }

    private void StartSuccessProcess()
    {
        this._state = State.Transcending;
        this._audio.Stop();
        this._audio.PlayOneShot(this.successSFX);
        Invoke("LoadNextLevel", levelDelay);
    }

    private void StartDeathProcess()
    {
        this._state = State.Dying;
        this._audio.Stop();
        this._audio.PlayOneShot(this.deathSFX);
        Invoke("LoadFirstLevel", deathDelay);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            this._audio.Stop();
        }
    }

    private void ApplyThrust()
    {
        this._rigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!this._audio.isPlaying)
        {
            this._audio.PlayOneShot(this.mainEngineSFX);
        }
    }

    private void RespondToRotateInput()
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

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(this._currentSceneIndex + 1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
