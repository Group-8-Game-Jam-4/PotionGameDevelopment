using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeCollider : MonoBehaviour
{
    bool interactable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && interactable == true)
        {
            Debug.Log("attempting to switch scenes");
            interactable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
            Debug.Log(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = false;
            Debug.Log(interactable);
        }
    }
}
