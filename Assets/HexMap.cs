using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HexMap : MonoBehaviour {
  public GameObject hexPrefab;
  public GameObject selectionPrefab;

  Dictionary<HexCoord, Hex> hexes;

  HexCoord selectionCenter;
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
    if (selection == null) {
      if (Input.GetMouseButtonUp(0)) {
        SelectHex();
      }
    } else {
      if (selection.CurrentState == Selection.State.DOWN) {
          FinishSelection();
      }
    }
  }

  public Hex HexAtPosition(Vector3 position) {
    HexCoord hexCoord = HexCoord.FromWorldPosition(position, 0.7f);
    if (hexes.ContainsKey(hexCoord)) {
      return hexes[hexCoord];
    }
    return null;
  }

  void SelectHex() {
    Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Hex clickedHex = HexAtPosition(clickedPosition);
    if (clickedHex != null && clickedHex.Position.Neighbors.All(neighbor => hexes.ContainsKey(neighbor))) {
      GameObject go = Instantiate(selectionPrefab, clickedHex.transform);
      selection = go.GetComponent<Selection>();
      foreach (Hex neighbor in clickedHex.Position.Neighbors.Select(pos => hexes[pos])) {
        neighbor.transform.SetParent(go.transform);
        neighbor.Selected = true;
        selection.Add(neighbor);
        selection.map = this;
      }
      selectionCenter = clickedHex.Position;
    }
  }

  void FinishSelection() {
    foreach (Hex hex in selection.Selected) {
      hex.transform.SetParent(transform);
      hex.transform.localEulerAngles = Vector3.zero;
      SetHexCoord(hex, hex.Position.Rotate(selection.CurrentRotation, selectionCenter));
      hex.Selected = false;
    }
    Destroy(selection.gameObject);
    selection = null;
  }

  void SetHexCoord(Hex hex, HexCoord coord) {
    hex.Position = coord;
    hexes[coord] = hex;
  }

  List<HexCoord> MapCoordinates() {
    List<HexCoord> result = new List<HexCoord>();
    for (int x = -2; x <= 2; x++) {
      for (int y = -2; y <= 3; y++) {
        for (int z = -3; z <= 2; z++) {
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
