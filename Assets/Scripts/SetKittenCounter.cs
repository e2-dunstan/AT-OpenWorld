using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKittenCounter : MonoBehaviour
{
    public GameObject congrats;
    private bool gameOver = false;
    public KittenController[] kittens;

	void Update ()
    {
        int numFollowing = 0;
        foreach(KittenController kitten in kittens)
        {
            if (kitten.following)
            {
                numFollowing++;
            }
        }
        GetComponent<Text>().text = numFollowing.ToString() + " / 9";

        if (numFollowing == 9 && !gameOver)
        {
            gameOver = true;
            congrats.SetActive(true);
            StartCoroutine(EndGame());
        }
	}

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
