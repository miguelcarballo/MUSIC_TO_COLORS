using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPauseScript : MonoBehaviour
{
    public void chooseOption()
    {
        if (gameObject.name.Equals("btnContinue"))
        {
            SessionHandler.unpause();
        }
        else if (gameObject.name.Equals("btnBackToMenu"))
        {
            SessionHandler.backToMenu();
        }

        
   

    }
}
