using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : MonoBehaviour
{
    Vector3 knifeInitPos;
    [SerializeField] WorldMove WorldMove;
    bool knifeInForwardPosition = false;
    [SerializeField] AudioSource EnemyKillNoise;
    int enemiesKilledCounter = 0;
    public bool allEnemiesKilled = false;
    [SerializeField] AudioSource EnemyKillNoiseWithReverb;
    [SerializeField] GameObject DriverSideImage;
    [SerializeField] GameObject PassengerSideImage;
    [SerializeField] GameObject DashImage;
    [SerializeField] GameObject Player;

    // TODO: make enemy appear every random amount of seconds and make them make a noise, then start a timer that will kill you if you dont stab them
    // TODO: listen for the stab


    void Start()
    {
        knifeInitPos = gameObject.transform.position;
    }

    void Update()
    {
        // if (!WorldMove.playerShouldCurrentlyBeStopped) return;

        if (Input.GetKeyDown("s"))
        {
            // move knife to init pos
            gameObject.transform.position = knifeInitPos;
            knifeInForwardPosition = false;
        }
        else if (Input.GetKeyDown("w"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "enemy" && !knifeInForwardPosition)
                {
                    enemiesKilledCounter++;
                    hit.transform.gameObject.SetActive(false);
                    if (enemiesKilledCounter == WorldMove.StopPoints[WorldMove.stopPointIndex].transform.GetChild(5).transform.childCount)
                    {
                        EnemyKillNoiseWithReverb.Play();
                    }
                    else
                    {
                        EnemyKillNoise.Play();
                    }

                    if (enemiesKilledCounter == WorldMove.StopPoints[WorldMove.stopPointIndex].transform.GetChild(5).transform.childCount)
                    {
                        Debug.Log("good!");
                        allEnemiesKilled = true;
                        enemiesKilledCounter = 0;
                    }
                }
            }

            knifeInForwardPosition = true;

            // move knife to stabbed pos
            gameObject.transform.position = new Vector3(knifeInitPos.x, knifeInitPos.y, knifeInitPos.z + 0.2f);


        }

        if (Input.GetKeyDown("a"))
        {
            DriverSideImage.SetActive(true);
            DashImage.SetActive(false);
            PassengerSideImage.SetActive(false);

            Player.transform.Rotate(0.0f, -90f, 0.0f, Space.Self);
        }

        if (Input.GetKeyDown("d"))
        {
            PassengerSideImage.SetActive(true);
            DriverSideImage.SetActive(false);
            DashImage.SetActive(false);

            Player.transform.Rotate(0.0f, 90f, 0.0f, Space.Self);
        }

        if (Input.GetKeyUp("a"))
        {
            DashImage.SetActive(true);
            DriverSideImage.SetActive(false);
            PassengerSideImage.SetActive(false);

            Player.transform.Rotate(0.0f, 90f, 0.0f, Space.Self);
        }

        if (Input.GetKeyUp("d"))
        {
            DashImage.SetActive(true);
            DriverSideImage.SetActive(false);
            PassengerSideImage.SetActive(false);

            Player.transform.Rotate(0.0f, -90f, 0.0f, Space.Self);
        }
    }
}
