using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q4SpriteRendererSprite : Q4ComponentPlayerList<Sprite>
{
    [SerializeField] private SpriteRenderer renderer;

    protected override Sprite GetCurrentListItem()
    {
        return renderer.sprite;
    }

    protected override void SetCurrentListItem(Sprite current)
    {
        renderer.sprite = current;
    }
}
