using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public GameObject hintText;
    public bool inRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hintText.SetActive(true);
        inRange = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hintText.SetActive(false);
        inRange = false;
    }
}
