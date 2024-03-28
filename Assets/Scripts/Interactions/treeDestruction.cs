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
    public float regrowTime = 180f;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;

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
        }

        if (Input.GetMouseButtonDown(0) && enterTrigger == true && !isBroken)
        {
            //Put players animation here hwne you get it
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
}
