using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//you need to actually comment on what you do instead of just writing spaghetti code idiot.
public class ColourPicker : MonoBehaviour
{
    public TMP_Dropdown coloursDropdown;
    public static string colourCode = "#ff0000";

    public void GetValue()
    {
        //gets the option the player selected
        int option = coloursDropdown.value;

        //picks colour code from option selected
        switch (option)
        {
            case 0:
                colourCode = "#ff0000";
                break;
            case 1:
                colourCode = "#ff7700";
                break; 
            case 2:
                colourCode = "#ffdd00";
                break;
            case 3:
                colourCode = "#28f735";
                break;
            case 4:
                colourCode = "#0038e0";
                break;
            case 5:
                colourCode = "#7400e0";
                break;
            case 6:
                colourCode = "#ff69ed";
                break;
            default:
                break;
        }
    }
}
