using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaMove : MonoBehaviour
{
    [SerializeField] WorldMove WorldMove;

    public void RestartLevel()
    {
        SceneManager.LoadScene("Playground");
    }

    public void StartUnderwater()
    {
        WorldMove.worldSpeed = 35f;
        WorldMove.yPosForWorldToMoveTowards = 6f;
        WorldMove.EntireWorld.transform.position = new Vector3(0, 6f, -4552.42f);
        WorldMove.EngineIdle.pitch = WorldMove.initialEngineIdlePitch;
        WorldMove.EngineIdleDistortion.distortionLevel = WorldMove.initialEngineIdleDistortionLevel;
        WorldMove.stopPointIndex = 10;
        WorldMove.UnderwaterFilter.SetActive(true);
        ColorUtility.TryParseHtmlString("#0F566C", out Color blueFromHex);
        RenderSettings.fogColor = blueFromHex;
        RenderSettings.fogEndDistance = 200f;
        WorldMove.BubbleEffects.SetActive(true);
        WorldMove.SnowEffects.SetActive(false);
    }

}
