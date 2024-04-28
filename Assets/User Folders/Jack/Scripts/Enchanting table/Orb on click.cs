using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbOnClick : MonoBehaviour
{
    public GameObject orbHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        EnchantingOrbs orbIncrement = orbHandler.GetComponent<EnchantingOrbs>();
        orbIncrement.bubblesPopped += 1;
    }
}
