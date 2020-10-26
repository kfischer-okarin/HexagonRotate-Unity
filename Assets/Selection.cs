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
  float dragStartAngle;
  float dragStartRotation;
  float rotationVelocity;

  public State CurrentState {
    get { return currentState; }
  }

  void Awake() {
    selected = new HashSet<Hex>();
  }

  float AngleDiff(float start, float end) {
    float normalizedStart = start;
    float normalizedEnd = end;
    // Rotate all angles into 0 ~ 180 segment to simplify diff calc
    while (Mathf.Abs(normalizedEnd - normalizedStart) > 180) {
      normalizedStart = (normalizedStart + 90) % 360f;
      normalizedEnd = (normalizedEnd + 90) % 360f;
    }
    return normalizedEnd - normalizedStart;
  }

  float SnapRotation(float angle) {
    return Mathf.Round((angle % 360f) / 60f) * 60f;
  }

  Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

  Vector3 SelectionCenter => transform.TransformPoint(Vector3.zero);

  Vector3 MouseOffsetFromSelectionCenter => MouseWorldPosition - SelectionCenter;

  bool ClickedInSelectedRing {
    get {
      Vector3 offsetWithoutTransform = transform.InverseTransformVector(MouseOffsetFromSelectionCenter);
      Vector3 clickPositionBeforeTransform = transform.parent.position + offsetWithoutTransform;
      Hex clickedHex = map.HexAtPosition(clickPositionBeforeTransform);
      return selected.Contains(clickedHex);
    }
  }

  float MouseAngle {
    get {
      float result = Vector2.Angle(Vector2.up, MouseOffsetFromSelectionCenter);
      if (MouseOffsetFromSelectionCenter.x > 0) {
        result = 360f - result;
      }
      return result;
    }
  }

  public float CurrentRotation {
    get { return transform.eulerAngles.z; }
    private set {
      float newValue = value % 360f;
      if (newValue < 0) {
        newValue += 360f;
      }
      transform.eulerAngles = new Vector3(0, 0, newValue);
    }
  }

  float TargetRotation {
    get {
      float dragRotation = AngleDiff(dragStartAngle, MouseAngle);
      float snappedDragRotation = SnapRotation(dragRotation);
      return dragStartRotation + snappedDragRotation;
    }
  }

  float DiffToTargetRotation => AngleDiff(CurrentRotation, TargetRotation);

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
            dragStartAngle = MouseAngle;
            dragStartRotation = SnapRotation(CurrentRotation);
          } else if (Input.GetMouseButtonUp(0)) {
            currentState = State.SINKING;
          }
          break;
        }
      case State.DRAGGING:
        {
          UpdateRotationVelocity();
          CurrentRotation += rotationVelocity;
          HandleSnap();
          break;
        }
      case State.SINKING:
        {
          Sink();
          break;
        }
    }
  }

  void UpdateRotationVelocity() {
    float force = DiffToTargetRotation;
    float damping = -rotationVelocity * 10f;
    rotationVelocity += Time.deltaTime * (force + damping);
  }

  void HandleSnap() {
    if (Mathf.Abs(DiffToTargetRotation) < 1 && Mathf.Abs(rotationVelocity) < 0.1f) {
      CurrentRotation = TargetRotation;
      rotationVelocity = 0;
      if (!Input.GetMouseButton(0)) {
        currentState = State.UP;
      }
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
