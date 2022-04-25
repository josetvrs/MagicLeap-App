using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using UnityEngine.SceneManagement;

public class FingerClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "AugmentedCell")
        {
            SceneManager.LoadScene("CellInstructions");
        }
        else if(col.gameObject.name == "ContinueButton1")
        {
            SceneManager.LoadScene("RobotScene");
        }
        else if(col.gameObject.name == "ManualMode")
        {
            SceneManager.LoadScene("ManualScene");
        }
        else if(col.gameObject.name == "RobotTwin")
        {
            SceneManager.LoadScene("TwinScene");
        }
    }
}
