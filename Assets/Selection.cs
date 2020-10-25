using System.Collections.Generic;

using UnityEngine;

public class Selection : MonoBehaviour {
  public HexMap map;
  HashSet<Hex> selected;

  public void Add(Hex hex) {
    selected.Add(hex);
  }

  public HashSet<Hex> Selected {
    get { return selected; }
  }

  public enum State {
    RISING,
    UP,
    DRAGGING,
    SINKING,
    DOWN
  }
  State currentState = State.RISING;

  public State CurrentState {
    get { return currentState; }
  }

  void Awake() {
    selected = new HashSet<Hex>();
  }

  // Update is called once per frame
  void Update() {
    switch (currentState) {
      case State.RISING:
        {
          Rise();
          break;
        }
      case State.UP:
        {
          if (Input.GetMouseButtonDown(0) && ClickedInSelectedRing) {
            currentState = State.DRAGGING;
          } else if (Input.GetMouseButtonUp(0)) {
            currentState = State.SINKING;
          }
          break;
        }
      case State.SINKING:
        {
          Sink();
          break;
        }
    }
  }

  bool ClickedInSelectedRing {
    get {
      Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector3 selectionCenter = transform.TransformPoint(Vector3.zero);
      Vector3 offsetFromSelection = clickedPosition - selectionCenter;

      Vector3 offsetWithoutTransform = transform.InverseTransformVector(offsetFromSelection);
      Vector3 clickPositionBeforeTransform = transform.parent.position + offsetWithoutTransform;
      Hex clickedHex = map.HexAtPosition(clickPositionBeforeTransform);
      return selected.Contains(clickedHex);
    }
  }

  const float FLOAT_HEIGHT = 0.3f;
  const float RISE_SPEED = 2f;

  void Rise() {
    Vector3 newPosition = transform.localPosition + Time.deltaTime * RISE_SPEED * Vector3.up;
    if (newPosition.y >= FLOAT_HEIGHT) {
      newPosition.y = FLOAT_HEIGHT;
      currentState = State.UP;
    }
    transform.localPosition = newPosition;
  }

  void Sink() {
    Vector3 newPosition = transform.localPosition + Time.deltaTime * RISE_SPEED * Vector3.down;
    if (newPosition.y <= 0) {
      newPosition.y = 0;
      currentState = State.DOWN;
    }
    transform.localPosition = newPosition;
  }
}
