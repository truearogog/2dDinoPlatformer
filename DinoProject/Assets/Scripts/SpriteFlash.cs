using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    private Material flashMaterial;
    private Material defaultMaterial;
    public float flashTime;
    private SpriteRenderer sr;

    void Start()
    {
        flashMaterial = Resources.Load<Material>("Materials/FlashMaterial");
        sr = GetComponent<SpriteRenderer>();
        defaultMaterial = sr.material;
    }

    public void Flash()
    {
        sr.material = flashMaterial;
        Invoke("ResetMaterial", flashTime);
    }

    private void ResetMaterial()
    {
        sr.material = defaultMaterial;
    }
}
