using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimate : MonoBehaviour
{
    private Material material;
    private float targetFloat;
    private float animateSpeed;
    private string propertyName;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void ChangeFloat(string propertyName, float targetFloat, float animateSpeed)
    {
        this.propertyName = propertyName;
        this.targetFloat = targetFloat;
        this.animateSpeed = animateSpeed;
    }

    void Update()
    {
        material.SetFloat(propertyName, Mathf.Lerp(material.GetFloat(propertyName), targetFloat, animateSpeed));
    }
}
