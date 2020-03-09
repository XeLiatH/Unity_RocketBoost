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


    bool isTransitioning = false;
    bool collisionsDisabled = false;

    void Start()
    {
        rocketRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionsDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                StartSuccessProcess();
                break;
            default:
                StartDeathProcess();
                break;
        }
    }

    void StartSuccessProcess()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(this.successSFX);
        successParticles.Play();
        Invoke("LoadNextLevel", levelDelay);
    }

    void StartDeathProcess()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        deathParticles.Play();
        Invoke("LoadFirstLevel", deathDelay);
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopThrust();
        }
    }

    void ApplyThrust()
    {
        rocketRigidbody.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }

        mainEngineParticles.Play();
    }

    void StopThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void RespondToRotateInput()
    {
        // remove rotation due to physics
        rocketRigidbody.angularVelocity = Vector3.zero;

        Vector3 rotationThisFrame = Vector3.forward * rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-rotationThisFrame);
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }
}
