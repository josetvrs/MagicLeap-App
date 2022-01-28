using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffStats : MonoBehaviour
{
    public GameObject Canvas;
    int counter;
    public void ShowHideCanvas()
    {
        counter++;
        if (counter % 2 == 1)
        {
            Canvas.gameObject.SetActive(true);
        } else
        {
            Canvas.gameObject.SetActive(false);
        }
    }
}