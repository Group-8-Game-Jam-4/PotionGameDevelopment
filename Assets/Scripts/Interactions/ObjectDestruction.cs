using System.Collections;
using UnityEngine;

public class ObjectDestruction : MonoBehaviour
{
    public int objectHealth = 100;
    public bool enterTrigger = false;
    public ParticleSystem damageParticle;
    public ParticleSystem destroyParticle;
    public SpriteRenderer spriteRenderer;
    public Sprite brokenSprite;
    public Sprite grownSprite;
    public GameObject dropItemPrefab;
    public float regrowTime = 180f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;
    public float spawnRadius = 2f;

    private bool isBroken = false;
    private Quaternion initialRotation;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (objectHealth <= 0 && !isBroken)
        {
            destroyParticle.Play();
            spriteRenderer.sprite = brokenSprite;
            isBroken = true;
            StartCoroutine(Regrow());
            InstantiateItems();

            ParticleSystem.MainModule mainModule = destroyParticle.main;
            mainModule.loop = false;
        }

        if (Input.GetMouseButtonDown(0) && enterTrigger && !isBroken)
        {
            objectHealth -= 20;
            damageParticle.Play();

            StartCoroutine(Shake());
        }
    }


    IEnumerator Regrow()
    {
        yield return new WaitForSeconds(regrowTime);
        spriteRenderer.sprite = grownSprite;
        isBroken = false;
        objectHealth = 100;
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

    void InstantiateItems()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            GameObject woodItem = Instantiate(dropItemPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(ScaleItem(woodItem));
        }
    }


    IEnumerator ScaleItem(GameObject woodItem)
    {
        Vector3 originalScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        float duration = 0.5f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            woodItem.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        woodItem.transform.localScale = targetScale;
    }


}
