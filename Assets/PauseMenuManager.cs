using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menuObject;
    public bool isActive = false;



    void Update()
    {
        if (isActive)
        {
            menuObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            menuObject.SetActive(false);
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
        }
    }


    public void ResumeButtonAction()
    {
        isActive = !isActive;
    }
}
