using System.Collections.Generic;

using UnityEngine;

public class Selection : MonoBehaviour {
  List<Hex> selected;

  public void Add(Hex hex) {
    selected.Add(hex);
  }

  public List<Hex> Selected {
    get { return selected; }
  }

  public enum State {
    RISING,
    UP,
    SINKING,
    DOWN
  }
  State currentState = State.RISING;

  public State CurrentState {
    get { return currentState; }
  }

  void Awake() {
    selected = new List<Hex>();
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
          if (Input.GetMouseButtonDown(0)) {
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
