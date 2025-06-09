using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : MonoBehaviour
{
    Vector3 knifeInitPos;
    [SerializeField] WorldMove WorldMove;
    bool knifeInForwardPosition = false;
    [SerializeField] AudioSource EnemyKillNoise;


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
                    hit.transform.gameObject.SetActive(false);
                    EnemyKillNoise.Play();
                }
            }

            knifeInForwardPosition = true;

            // move knife to stabbed pos
            gameObject.transform.position = new Vector3(knifeInitPos.x, knifeInitPos.y, knifeInitPos.z + 0.2f);


        }


    }
}
