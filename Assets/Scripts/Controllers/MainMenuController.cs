using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject controls;
    public GameObject loading;
    public GameObject instructions;


    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (controls.activeInHierarchy)
            {
                instructions.SetActive(false);
                loading.SetActive(true);
                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            }
            else
            {
                controls.SetActive(true);
            }
        }
	}
}
