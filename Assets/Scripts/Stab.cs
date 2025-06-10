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
    public bool EXEC_SideEnemySpawner = false;
    [SerializeField] GameObject LeftEnemy;
    [SerializeField] GameObject RightEnemy;
    [SerializeField] AudioSource SideEnemyAppearedSound;
    bool currentlyHeldRight = false;
    bool currentlyHeldLeft = false;


    //* MVP
    // TODO: 


    //* nice-to-have
    // TODO: animation for switching to looking left/right


    void Start()
    {
        knifeInitPos = gameObject.transform.position;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
                else if (hit.transform.gameObject.tag == "sideEnemy" && !knifeInForwardPosition)
                {
                    hit.transform.gameObject.SetActive(false);
                    EnemyKillNoise.Play();
                }
            }

            knifeInForwardPosition = true;

            // move knife to stabbed pos
            if (currentlyHeldRight)
            {
                gameObject.transform.position = new Vector3(knifeInitPos.x + 0.3f, knifeInitPos.y, knifeInitPos.z);
            }
            else if (currentlyHeldLeft)
            {
                gameObject.transform.position = new Vector3(knifeInitPos.x - 0.3f, knifeInitPos.y, knifeInitPos.z);
            }
            else
            {
                gameObject.transform.position = new Vector3(knifeInitPos.x, knifeInitPos.y, knifeInitPos.z + 0.3f);
            }


        }

        if (Input.GetKeyDown("a") && !currentlyHeldRight)
        {
            currentlyHeldLeft = true;
            DriverSideImage.SetActive(true);
            DashImage.SetActive(false);
            PassengerSideImage.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (LeftEnemy.activeSelf)
            {
                if (!SideEnemyAppearedSound.isPlaying)
                {
                    SideEnemyAppearedSound.Play();
                }
            }

            Player.transform.Rotate(0.0f, -90f, 0.0f, Space.Self);
            knifeInitPos = gameObject.transform.position;
        }
        else if (Input.GetKeyDown("d") && !currentlyHeldLeft)
        {
            currentlyHeldRight = true;
            PassengerSideImage.SetActive(true);
            DriverSideImage.SetActive(false);
            DashImage.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (RightEnemy.activeSelf)
            {
                if (!SideEnemyAppearedSound.isPlaying)
                {
                    SideEnemyAppearedSound.Play();
                }
            }

            Player.transform.Rotate(0.0f, 90f, 0.0f, Space.Self);
            knifeInitPos = gameObject.transform.position;
        }

        if (Input.GetKeyUp("a") && currentlyHeldLeft)
        {
            currentlyHeldLeft = false;
            DashImage.SetActive(true);
            DriverSideImage.SetActive(false);
            PassengerSideImage.SetActive(false);

            Player.transform.Rotate(0.0f, 90f, 0.0f, Space.Self);
            knifeInitPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 0.3f);
        }

        else if (Input.GetKeyUp("d") && currentlyHeldRight)
        {
            currentlyHeldRight = false;
            DashImage.SetActive(true);
            DriverSideImage.SetActive(false);
            PassengerSideImage.SetActive(false);

            Player.transform.Rotate(0.0f, -90f, 0.0f, Space.Self);
            knifeInitPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 0.3f);
        }

        if (EXEC_SideEnemySpawner)
        {
            StartCoroutine(SideEnemySpawner());
        }
    }

    IEnumerator SideEnemySpawner()
    {
        EXEC_SideEnemySpawner = false;
        int random = Random.Range(100, 150);
        yield return new WaitForSeconds(random);
        int randomSide = Random.Range(0, 2);
        if (randomSide == 0)
        {
            LeftEnemy.SetActive(true);
        }
        else if (randomSide == 1)
        {
            RightEnemy.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        if (LeftEnemy.activeSelf || RightEnemy.activeSelf)
        {
            WorldMove.DeathScreenEnemy.SetActive(true);
            if (!WorldMove.DeathSound.isPlaying)
            {
                WorldMove.DeathSound.Play();
            }
            gameObject.SetActive(false);
            WorldMove.PixelStylizerCamera.pixelSize = 3;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.Log("killed side enemy in time");
        }
        EXEC_SideEnemySpawner = true;
    }
}
