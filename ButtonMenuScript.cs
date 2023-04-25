using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenuScript : MonoBehaviour
{
    public void playSelected()
    {
        if (gameObject.name.Equals("EXIT"))
        {
            SessionHandler.quitApp();
        }
        else
        {
            SessionHandler.nameCurrentPiece = gameObject.name;
            Debug.Log("*******NAME OBJ = " + gameObject.name);
            SessionHandler.isPlaying = true;
        }

        // GameObject canvas = GameObject.Find("Canvas");

    }
}
