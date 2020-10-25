using UnityEngine;

public class Hex : MonoBehaviour {
    HexCoord position;
    public HexCoord Position {
        get { return position; }
        set {
            this.position = value;
            transform.position = value.Position(0.7f);
        }
    }
}
