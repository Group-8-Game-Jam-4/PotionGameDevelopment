using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class itemCollect : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI hintText;
    public ParticleSystem burstEffect;

    public float moveSpeed = 5f;
    public bool isPickedUp = false;
    private bool inRange;
    public bool hasInteracted = false;

    private void Awake()
    {
        player = GameObject.Find("Player");
        GameObject canvas = GameObject.Find("HintText");
        //hintText = canvas.transform.Find("ItemHint").GetComponent<TextMeshProUGUI>();
        burstEffect = player.GetComponentInChildren<ParticleSystem>();
    }


    void Update()
    {
        if (inRange && !isPickedUp)
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
            //hintText.gameObject.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            //hintText.gameObject.SetActive(false);
            inRange = false;
        }
    }
}
