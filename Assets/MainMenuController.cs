using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject controls;

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (controls.activeInHierarchy)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            }
            else
            {
                controls.SetActive(true);
            }
        }
	}
}
