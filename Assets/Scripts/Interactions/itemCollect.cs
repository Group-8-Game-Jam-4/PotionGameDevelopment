using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollect : MonoBehaviour
{
    public GameObject player;
    public GameObject hintText;
    public float moveSpeed = 5f;
    public bool isPickedUp = false;
    private bool inRange;
    public bool hasInteracted = false;
    public ParticleSystem burstEffect;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRange && !isPickedUp)
        {
            isPickedUp = true;
            hasInteracted = true;
        }

        if (isPickedUp)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
    }

    public void RemoveItem()
    {
            transform.position = player.transform.position;
            burstEffect.Play();
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player && !isPickedUp)
        {
            hintText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            hintText.SetActive(false);
            inRange = false;
        }
    }
}
