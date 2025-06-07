using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMove : MonoBehaviour
{
    [SerializeField] GameObject EntireWorld;
    Vector3 initialEntireWorldPos;
    void Start()
    {
        initialEntireWorldPos = EntireWorld.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        EntireWorld.transform.position = Vector3.MoveTowards(EntireWorld.transform.position, new Vector3(0f, 0f, -1000f), 10f * Time.deltaTime);
    }
}
