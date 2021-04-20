using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScaling : MonoBehaviour
{
    // Start is called before the first frame update
    Texture Texture;
    void Start()
    {
        transform.localScale = new Vector3(1, 4, 4);
        print("Size is " + Texture.width + " by " + Texture.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
