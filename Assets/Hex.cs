using UnityEngine;

public class Hex : MonoBehaviour {
    public Sprite defaultSprite;
    public Sprite selectedSprite;

    HexCoord position;
    public HexCoord Position {
        get { return position; }
        set {
            this.position = value;
            transform.position = value.WorldPosition(0.7f);
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
    }
}
