﻿using UnityEngine;

public class Hex : MonoBehaviour {
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    HexCoord position;
    public HexCoord Position {
        get { return position; }
        set {
            this.position = value;
            transform.position = value.WorldPosition(HexMap.HEX_SIZE);
        }
    }

    bool selected;

    public bool Selected {
        get { return selected; }
        set {
            selected = value;
            spriteRenderer.sprite = value ? selectedSprite : defaultSprite;
        }
    }

    SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
        spriteRenderer.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
    }
}
