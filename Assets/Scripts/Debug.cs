using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug : MonoBehaviour
{
    [SerializeField] WorldMove WorldMove;

    public void RestartLevel()
    {
        SceneManager.LoadScene("Playground");
    }

    public void StartUnderwater()
    {
        WorldMove.worldSpeed = 35f;
        WorldMove.EntireWorld.transform.position = new Vector3(0, 0, -4552.42f);
        WorldMove.EngineIdle.pitch = WorldMove.initialEngineIdlePitch;
        WorldMove.EngineIdleDistortion.distortionLevel = WorldMove.initialEngineIdleDistortionLevel;
        WorldMove.stopPointIndex = 10;
    }

}
