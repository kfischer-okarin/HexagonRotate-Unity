using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {
    public GameObject hexPrefab;

    void Awake() {
        foreach (HexCoord coord in MapCoordinates()) {
            GameObject go = Instantiate(hexPrefab, Vector3.zero, Quaternion.identity, transform);
            go.GetComponent<Hex>().Position = coord;
        }

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
