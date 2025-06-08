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
    [SerializeField] GameObject Player;
    bool EXEC_redLightSwitch = false;
    bool playerShouldCurrentlyBeStopped = false;
    [SerializeField] GameObject DeathScreen;

    void Start()
    {
        initialEntireWorldPos = EntireWorld.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            EntireWorld.transform.position = Vector3.MoveTowards(EntireWorld.transform.position, new Vector3(0f, 0f, -1000f), 20f * Time.deltaTime);
            if (playerShouldCurrentlyBeStopped)
            {
                Debug.Log("you have been t-boned hehe");
                DeathScreen.SetActive(true);
            }
        }

        distanceFromNextStopPoint = Vector3.Distance(Player.transform.position, StopPoints[stopPointIndex].transform.position);

        if (distanceFromNextStopPoint < 30f)
        {
            EXEC_redLightSwitch = true;
        }

        if (distanceFromNextStopPoint < 60f)
        {
            Debug.Log("begin countdown to switch to green and begin stab portion");
        }

        if (EXEC_redLightSwitch)
        {
            Debug.Log("YEAH!!!!!!!!");

            // red light
            StopPoints[stopPointIndex].transform.GetChild(3).transform.GetChild(0).GetComponent<Light>().intensity = 3;

            // yellow light
            StopPoints[stopPointIndex].transform.GetChild(2).transform.GetChild(0).GetComponent<Light>().intensity = 0;

            EXEC_redLightSwitch = false;
            playerShouldCurrentlyBeStopped = true;
        }
    }
}
