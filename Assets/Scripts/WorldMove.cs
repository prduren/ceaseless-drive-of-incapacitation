using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMove : MonoBehaviour
{
    [SerializeField] GameObject EntireWorld;
    Vector3 initialEntireWorldPos;
    [SerializeField] List<GameObject> StopPoints;
    int stopPointIndex = 0;
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

    void Start()
    {
        initialEntireWorldPos = EntireWorld.transform.position;
        worldSpeed = 20f;
        initialEngineIdlePitch = EngineIdle.pitch;
        initialEngineIdleDistortionLevel = EngineIdleDistortion.distortionLevel;
        heldEngineIdlePitch = EngineIdle.pitch;
        heldEngineIdleDistortionLevel = EngineIdleDistortion.distortionLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            EngineIdle.pitch = heldEngineIdlePitch;
            EngineIdleDistortion.distortionLevel = heldEngineIdleDistortionLevel;
            EntireWorld.transform.position = Vector3.MoveTowards(EntireWorld.transform.position, new Vector3(0f, 0f, -10000f), worldSpeed * Time.deltaTime);
            if (playerShouldCurrentlyBeStopped)
            {
                DeathScreen.SetActive(true);
            }
        }
        else
        {
            EngineIdle.pitch = initialEngineIdlePitch;
            EngineIdleDistortion.distortionLevel = initialEngineIdleDistortionLevel;
        }

        Debug.Log(stopPointIndex);
        distanceFromNextStopPoint = Vector3.Distance(Player.transform.position, StopPoints[stopPointIndex].transform.position);

        if (distanceFromNextStopPoint < 40f)
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
        }

        if (EXEC_redLightSwitch)
        {

            // red light enable
            StopPoints[stopPointIndex].transform.GetChild(3).transform.GetChild(0).GetComponent<Light>().intensity = 3;

            // yellow light disable
            StopPoints[stopPointIndex].transform.GetChild(2).transform.GetChild(0).GetComponent<Light>().intensity = 0;

            playerShouldCurrentlyBeStopped = true;

            EXEC_redLightSwitch = false;
        }
    }

    private IEnumerator TimerForGreenLight()
    {
        Debug.Log("you should stop now");

        // yellow light enable
        StopPoints[stopPointIndex].transform.GetChild(2).transform.GetChild(0).GetComponent<Light>().intensity = 3;

        // green light disable
        StopPoints[stopPointIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<Light>().intensity = 0;

        EXEC_GreenLightFlag = false;
        yield return new WaitForSeconds(2f);
        EXEC_redLightSwitch = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // all the enemy faces
        StopPoints[stopPointIndex].transform.GetChild(5).transform.gameObject.SetActive(true);
        BeepAudio.Play();

        yield return new WaitForSeconds(10f);

        // green light enable
        StopPoints[stopPointIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<Light>().intensity = 3;
        // red light disable
        StopPoints[stopPointIndex].transform.GetChild(3).transform.GetChild(0).GetComponent<Light>().intensity = 0;

        BeepAudio.Stop();
        playerShouldCurrentlyBeStopped = false;
        stopPointIndex++;
        worldSpeed = worldSpeed + 10f;
        heldEngineIdlePitch += 0.2f;
        heldEngineIdleDistortionLevel += 0.1f;

        Cursor.visible = false;
    }
}
