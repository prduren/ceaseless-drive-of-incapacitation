using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool gameIsPaused = false;
    [SerializeField] GameObject PauseObject;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseObject.SetActive(true);
                gameIsPaused = true;
            }
            else if (gameIsPaused)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                PauseObject.SetActive(false);
                gameIsPaused = false;
            }

        }
    }
}
