using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {
    public GameObject hexPrefab;

    Dictionary<HexCoord, Hex> hexes;

    void Awake() {
        hexes = new Dictionary<HexCoord, Hex>();
        foreach (HexCoord coord in MapCoordinates()) {
            GameObject go = Instantiate(hexPrefab, Vector3.zero, Quaternion.identity, transform);
            Hex hex = go.GetComponent<Hex>();
            SetHexCoord(hex, coord);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            HexCoord clickedHexCoord = HexCoord.FromWorldPosition(clickedPosition, 0.7f);
            if (hexes.ContainsKey(clickedHexCoord)) {
                hexes[clickedHexCoord].Selected = !hexes[clickedHexCoord].Selected;
            }
        }
    }

    void SetHexCoord(Hex hex, HexCoord coord) {
        hex.Position = coord;
        hexes[coord] = hex;
    }

    List<HexCoord> MapCoordinates() {
        List<HexCoord> result = new List<HexCoord>();
        for (int x = -2; x <= 2; x++) {
            for (int y = -3; y <= 2; y++) {
                for (int z = -2; z <= 3; z++) {
                    if (x + y + z != 0) {
                        continue;
                    }
                    result.Add(new HexCoord(x, y, z));
                }
            }
        }
        return result;
    }
}
