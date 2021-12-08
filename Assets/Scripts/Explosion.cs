using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 2.0f);
        }
        Destroy(this.gameObject, 2.0f);
    }
}
