using UnityEngine;

// Hex Cube Coordinate
public struct HexCoord {
  public int x;
  public int y;
  public int z;

  public HexCoord(int x, int y, int z) {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  static float SQRT_3 = Mathf.Sqrt(3);

  public Vector2 Position(float size) {
    int q = x;
    int r = z;
    return new Vector2(
      size * 1.5f * q,
      size * (SQRT_3 / 2 * q + SQRT_3 * r)
    );
  }
}
