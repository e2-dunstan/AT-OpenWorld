using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetKittenCounter : MonoBehaviour
{
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
	}
}
