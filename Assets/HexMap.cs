using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class HexMap : MonoBehaviour {
  public GameObject hexPrefab;
  public GameObject selectionPrefab;

  Dictionary<HexCoord, Hex> hexesByCoord;

  HexCoord selectionCenter;
  Selection selection;

  public const float HEX_SIZE = 0.7f;

   public Hex HexAtPosition(Vector3 position) {
    HexCoord hexCoord = HexCoord.FromWorldPosition(position, HEX_SIZE);
    if (hexesByCoord.ContainsKey(hexCoord)) {
      return hexesByCoord[hexCoord];
    }
    return null;
  }

  void SetHexCoord(Hex hex, HexCoord coord) {
    hex.Position = coord;
    hexesByCoord[coord] = hex;
  }

  void BuildHexMap() {
    for (int x = -2; x <= 2; x++) {
      for (int y = -2; y <= 3; y++) {
        for (int z = -3; z <= 2; z++) {
          if (x + y + z != 0) {
            continue;
          }
          GameObject go = Instantiate(hexPrefab, Vector3.zero, Quaternion.identity, transform);
          Hex hex = go.GetComponent<Hex>();
          SetHexCoord(hex, new HexCoord(x, y, z));
        }
      }
    }
  }

  void Awake() {
    hexesByCoord = new Dictionary<HexCoord, Hex>();
    BuildHexMap();
  }

  void SelectHexesAround(Hex center) {
      GameObject go = Instantiate(selectionPrefab, center.transform);
      selection = go.GetComponent<Selection>();
      foreach (Hex neighbor in center.Position.Neighbors.Select(pos => hexesByCoord[pos])) {
        neighbor.transform.SetParent(go.transform);
        neighbor.Selected = true;
        selection.Add(neighbor);
        selection.map = this;
      }
      selectionCenter = center.Position;
  }

  void ResetRotation(Hex hex) {
    hex.transform.localEulerAngles = Vector3.zero;
  }

  void ApplyDragRotation(Hex hex) {
    SetHexCoord(hex, hex.Position.Rotate(selection.CurrentRotation, selectionCenter));
  }

  void FinishSelection() {
    foreach (Hex hex in selection.Selected) {
      hex.transform.SetParent(transform);
      ResetRotation(hex);
      ApplyDragRotation(hex);
      hex.Selected = false;
    }
    Destroy(selection.gameObject);
    selection = null;
  }

  void Update() {
    if (selection == null) {
      if (Input.GetMouseButtonUp(0)) {
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Hex clickedHex = HexAtPosition(clickedPosition);
        if (clickedHex != null && clickedHex.Position.Neighbors.All(neighbor => hexesByCoord.ContainsKey(neighbor))) {
          SelectHexesAround(clickedHex);
        }
      }
    } else {
      if (selection.CurrentState == Selection.State.FINISHED) {
          FinishSelection();
      }
    }
  }
}
