using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isStaticObject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }

    void Update()
    {
        if (!isStaticObject)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }
}
