using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite(bool isFlipped)
    {
        _spriteRenderer.flipY = isFlipped;
    }
}
