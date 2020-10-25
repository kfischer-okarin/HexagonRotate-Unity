using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HexMap : MonoBehaviour {
  public GameObject hexPrefab;
  public GameObject selectionPrefab;

  Dictionary<HexCoord, Hex> hexes;

  Selection selection;

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
      if (selection == null) {
        SelectHex();
      } else {

      }

    }
  }

  void SelectHex() {
    Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    HexCoord clickedHexCoord = HexCoord.FromWorldPosition(clickedPosition, 0.7f);
    if (hexes.ContainsKey(clickedHexCoord) && clickedHexCoord.Neighbors.All(neighbor => hexes.ContainsKey(neighbor))) {
      Hex clickedHex = hexes[clickedHexCoord];
      GameObject go = Instantiate(selectionPrefab, clickedHex.transform);
      foreach (Hex neighbor in clickedHex.Position.Neighbors.Select(pos => hexes[pos])) {
        neighbor.transform.SetParent(go.transform);
        neighbor.Selected = true;
      }
      selection = go.GetComponent<Selection>();
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
