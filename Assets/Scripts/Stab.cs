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


    void Start()
    {
        knifeInitPos = gameObject.transform.position;
    }

    void Update()
    {
        if (!WorldMove.playerShouldCurrentlyBeStopped) return;

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
                    EnemyKillNoise.Play();
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


    }
}
