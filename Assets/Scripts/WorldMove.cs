using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMove : MonoBehaviour
{
    [SerializeField] GameObject EntireWorld;
    Vector3 initialEntireWorldPos;
    [SerializeField] public List<GameObject> StopPoints;
    public int stopPointIndex = 0;
    float distanceFromNextStopPoint;
    float distanceFromPreviousStopPoint;
    [SerializeField] GameObject Player;
    bool EXEC_redLightSwitch = false;
    public bool playerShouldCurrentlyBeStopped = false;
    [SerializeField] GameObject DeathScreen;
    bool EXEC_GreenLightFlag = true;
    float worldSpeed;
    [SerializeField] AudioSource EngineIdle;
    float initialEngineIdlePitch;
    float heldEngineIdlePitch;
    [SerializeField] AudioDistortionFilter EngineIdleDistortion;
    float initialEngineIdleDistortionLevel;
    float heldEngineIdleDistortionLevel;
    [SerializeField] AudioSource BeepAudio;
    [SerializeField] public AudioSource DeathSound;
    [SerializeField] Stab Stab;
    [SerializeField] GameObject InstructionsPage;
    [SerializeField] public PixelStylizerCamera PixelStylizerCamera;
    [SerializeField] public GameObject DeathScreenEnemy;
    [SerializeField] ParticleSystem SnowParticles;
    [SerializeField] GameObject EndScreen;
    [SerializeField] GameObject gameEndLocation;
    float distanceFromGameEnd;
    [SerializeField] AudioSource UnderwaterSound;
    bool firstTimeToEnableSideEnemySpawner = false;
    [SerializeField] AudioSource AmbientTrack;

    //* MVP DONE
    // DECREASE MUSIC AS LEVEL GOES ON

    //* nice-to-haves
    // TODO: radio
    // TODO: screen shake on enemy kill

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        initialEntireWorldPos = EntireWorld.transform.position;
        worldSpeed = 35f; // OG is 20f // DEBUG
        initialEngineIdlePitch = EngineIdle.pitch;
        initialEngineIdleDistortionLevel = EngineIdleDistortion.distortionLevel;
        heldEngineIdlePitch = EngineIdle.pitch;
        heldEngineIdleDistortionLevel = EngineIdleDistortion.distortionLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !DeathScreen.activeSelf && !DeathScreenEnemy.activeSelf && !EndScreen.activeSelf)
        {

            if (!firstTimeToEnableSideEnemySpawner)
            {
                firstTimeToEnableSideEnemySpawner = true;
                Stab.EXEC_SideEnemySpawner = true;
            }

            PixelStylizerCamera.pixelSize = 6;
            InstructionsPage.SetActive(false);
            Stab.gameObject.SetActive(true);
            EngineIdle.pitch = heldEngineIdlePitch;
            EngineIdleDistortion.distortionLevel = heldEngineIdleDistortionLevel;
            EntireWorld.transform.position = Vector3.MoveTowards(EntireWorld.transform.position, new Vector3(0f, 0f, -10000f), worldSpeed * Time.deltaTime);
            if (playerShouldCurrentlyBeStopped)
            {
                DeathScreen.SetActive(true);
                StopAllCoroutines();
                if (!DeathSound.isPlaying)
                {
                    DeathSound.Play();

                }
                Stab.gameObject.SetActive(false);
                PixelStylizerCamera.pixelSize = 3;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
        }
        else
        {
            EngineIdle.pitch = initialEngineIdlePitch;
            EngineIdleDistortion.distortionLevel = initialEngineIdleDistortionLevel;
        }

        distanceFromNextStopPoint = Vector3.Distance(Player.transform.position, StopPoints[stopPointIndex].transform.position);

        if (distanceFromNextStopPoint < 46f)
        {
            if (EXEC_GreenLightFlag)
            {
                StartCoroutine(TimerForGreenLight());
            }
        }

        // ensure we're only setting previous stop point distance from player IF we're even past the first one
        if (stopPointIndex > 0)
        {
            distanceFromPreviousStopPoint = Vector3.Distance(Player.transform.position, StopPoints[stopPointIndex - 1].transform.position);
        }

        // when we're far enough awayjrom the previous point, go ahead and reset the flag for the coroutine to be ready to run on the next stop light
        if (distanceFromPreviousStopPoint > 41f && distanceFromPreviousStopPoint < 50f)
        {
            EXEC_GreenLightFlag = true;
            Stab.allEnemiesKilled = false;
        }

        if (EXEC_redLightSwitch)
        {

            // red light enable
            StopPoints[stopPointIndex].transform.GetChild(3).transform.GetChild(0).GetComponent<Light>().intensity = 4;

            // yellow light disable
            StopPoints[stopPointIndex].transform.GetChild(2).transform.GetChild(0).GetComponent<Light>().intensity = 0;

            playerShouldCurrentlyBeStopped = true;

            EXEC_redLightSwitch = false;
        }

        distanceFromGameEnd = Vector3.Distance(Player.transform.position, gameEndLocation.transform.position);
        if (distanceFromGameEnd < 30f)
        {
            EndScreen.SetActive(true);
            StopAllCoroutines();
            Stab.StopAllCoroutines();
            Stab.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            PixelStylizerCamera.pixelSize = 3;
            if (!UnderwaterSound.isPlaying)
            {
                UnderwaterSound.Play();
            }
        }
    }

    private IEnumerator TimerForGreenLight()
    {
        // yellow light enable
        StopPoints[stopPointIndex].transform.GetChild(2).transform.GetChild(0).GetComponent<Light>().intensity = 4;

        // green light disable
        StopPoints[stopPointIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<Light>().intensity = 0;

        EXEC_GreenLightFlag = false;
        yield return new WaitForSeconds(1f);
        EXEC_redLightSwitch = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // all the enemy faces
        StopPoints[stopPointIndex].transform.GetChild(5).transform.gameObject.SetActive(true);
        BeepAudio.Play();

        yield return new WaitForSeconds(7f);

        // green light enable
        StopPoints[stopPointIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<Light>().intensity = 4;
        // red light disable
        StopPoints[stopPointIndex].transform.GetChild(3).transform.GetChild(0).GetComponent<Light>().intensity = 0;

        BeepAudio.Stop();
        playerShouldCurrentlyBeStopped = false;
        stopPointIndex++;
        AmbientTrack.volume -= 0.2f;
        worldSpeed = worldSpeed + 5f;
        // SnowParticles.rateOverTime += 10;
        heldEngineIdlePitch += 0.2f;
        heldEngineIdleDistortionLevel += 0.07f;


        if (!Stab.allEnemiesKilled)
        {
            DeathScreenEnemy.SetActive(true);
            if (!DeathSound.isPlaying)
            {
                DeathSound.Play();
            }
            Stab.gameObject.SetActive(false);
            PixelStylizerCamera.pixelSize = 3;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
