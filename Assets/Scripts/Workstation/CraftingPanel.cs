using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingPanel : MonoBehaviour
{
    public GameObject input1;
    public GameObject input2;
    public TextMeshProUGUI input1Text;
    public TextMeshProUGUI input2Text;
    public GameObject workstation;
    public TextMeshProUGUI workstationText;
    public GameObject output;
    public TextMeshProUGUI outputText;
    public GameObject strike;
    public GameObject CraftingParent;
    public TextMeshProUGUI HarvestText;

    public void SetStrike(bool value)
    {
        strike.SetActive(value);
    }


}
