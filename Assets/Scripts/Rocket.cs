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

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rocketRigidbody;
    AudioSource audioSource;

    int currentSceneIndex;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        this.rocketRigidbody = GetComponent<Rigidbody>();
        this.audioSource = GetComponent<AudioSource>();
        this.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (this.state != State.Alive) { return; }

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
        this.state = State.Transcending;
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.successSFX);
        this.successParticles.Play();
        Invoke("LoadNextLevel", levelDelay);
    }

    private void StartDeathProcess()
    {
        this.state = State.Dying;
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.deathSFX);
        this.deathParticles.Play();
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
            this.audioSource.Stop();
            this.mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        this.rocketRigidbody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!this.audioSource.isPlaying)
        {
            this.audioSource.PlayOneShot(this.mainEngineSFX);
        }

        this.mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        this.rocketRigidbody.freezeRotation = true; // take manual control of rotation

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
        SceneManager.LoadScene(this.currentSceneIndex + 1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
