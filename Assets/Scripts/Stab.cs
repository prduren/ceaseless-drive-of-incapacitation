using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : MonoBehaviour
{
    Vector3 knifeInitPos;
    [SerializeField] WorldMove WorldMove;

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
        }
        else if (Input.GetKeyDown("w"))
        {
            // move knife to stabbed pos
            gameObject.transform.position = new Vector3(knifeInitPos.x, knifeInitPos.y, knifeInitPos.z + 0.2f);
        }
    }
}
