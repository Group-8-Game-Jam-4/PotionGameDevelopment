using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopEntry : MonoBehaviour
{
    public string sceneToLoad;
    public bool entered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to change scene");
            entered= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
    }

    void Update()
    {
        if (entered == true && (Input.GetKeyDown(KeyCode.E)))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
}
