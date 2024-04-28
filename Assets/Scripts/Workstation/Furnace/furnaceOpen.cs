using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class furnaceOpen : MonoBehaviour
{
    public bool entered = true;
    public GameObject furnaceCanvas;
    public furnaceController furnaceController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && entered) 
        {
            OpenFurnaceUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            furnaceController.furnaceLvl = 0;
            furnaceCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entered= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
        furnaceController.furnaceLvl = 0;
        furnaceCanvas.SetActive(false);
    }

    public void OpenFurnaceUI()
    {
        furnaceCanvas.SetActive(true);
    }
}
