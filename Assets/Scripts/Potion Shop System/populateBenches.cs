using UnityEngine;

public class populateBenches : MonoBehaviour
{
    public Sprite[] originalSprites; 
    public Sprite[] randomSprites;   

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void stockShelf()
    {
        int randomIndex = Random.Range(0, randomSprites.Length);
        spriteRenderer.sprite = randomSprites[randomIndex];
    }

    public void unstockShelf()
    {
        int originalIndex = Random.Range(0, originalSprites.Length);
        spriteRenderer.sprite = originalSprites[originalIndex];
    }
}
