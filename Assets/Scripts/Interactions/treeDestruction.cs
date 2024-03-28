using System.Collections;
using UnityEngine;

public class TreeDestruction : MonoBehaviour
{
    public int treeHealth = 100;
    public bool enterTrigger = false;
    public ParticleSystem damageParticle;
    public ParticleSystem destroyParticle;
    public SpriteRenderer treeRenderer;
    public Sprite brokenTreeSprite;
    public Sprite grownTreeSprite;
    public GameObject woodItemPrefab;
    public float regrowTime = 180f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;
    public float woodSpawnRadius = 2f;

    private bool isBroken = false;
    private Quaternion initialRotation;

    void Start()
    {
        treeRenderer = GetComponent<SpriteRenderer>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (treeHealth <= 0 && !isBroken)
        {
            destroyParticle.Play();
            treeRenderer.sprite = brokenTreeSprite;
            isBroken = true;
            StartCoroutine(RegrowTree());
            InstantiateWoodItems();
        }

        if (Input.GetMouseButtonDown(0) && enterTrigger && !isBroken)
        {
            // Put players animation here when you get it
            treeHealth -= 20;
            damageParticle.Play();

            StartCoroutine(Shake());
        }
    }

    IEnumerator RegrowTree()
    {
        yield return new WaitForSeconds(regrowTime);
        treeRenderer.sprite = grownTreeSprite;
        isBroken = false;
        treeHealth = 100;
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        Quaternion originalRotation = transform.rotation;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
            float x = Random.Range(-1f, 1f) * shakeIntensity * damper;
            float y = Random.Range(-1f, 1f) * shakeIntensity * damper;

            transform.rotation = new Quaternion(originalRotation.x + x, originalRotation.y + y, originalRotation.z, originalRotation.w);

            yield return null;
        }

        transform.rotation = initialRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enterTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enterTrigger = false;
    }

    void InstantiateWoodItems()
    {
        for (int i = 0; i < 5; i++)
        {
            // Generate random offset within the radius around the tree
            Vector3 randomOffset = Random.insideUnitCircle * woodSpawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            // Instantiate wood items at the random position
            GameObject woodItem = Instantiate(woodItemPrefab, spawnPosition, Quaternion.identity);

            // Start coroutine to scale the wood item
            StartCoroutine(ScaleWoodItem(woodItem));
        }
    }


    IEnumerator ScaleWoodItem(GameObject woodItem)
    {
        Vector3 originalScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        float duration = 0.5f; // Adjust the duration as needed

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            woodItem.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // Ensure that the wood item reaches the exact target scale
        woodItem.transform.localScale = targetScale;
    }


}
