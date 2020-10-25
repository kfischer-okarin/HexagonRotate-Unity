using System.Collections.Generic;

using UnityEngine;

public class Selection : MonoBehaviour {
  List<Hex> selected;
  enum State {
    RISING,
    UP,
    SINKING,
    DOWN
  }
  State state = State.RISING;

  void Awake() {
    selected = new List<Hex>();
  }

  // Update is called once per frame
  void Update() {
    switch (state) {
      case State.RISING:
        {
          Rise();
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
      state = State.UP;
    }
    transform.localPosition = newPosition;
  }
}
