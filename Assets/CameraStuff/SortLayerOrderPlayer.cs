using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerOrderPlayer : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderersSprites;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderersSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int order = Mathf.RoundToInt((transform.position.y + 0.5f) * -100);
        foreach(SpriteRenderer sr in spriteRenderersSprites)
        {
            sr.sortingOrder = order;
        }
    }
}
