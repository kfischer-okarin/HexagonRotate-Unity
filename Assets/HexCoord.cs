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

  public Vector2 WorldPosition(float size) {
    int q = x;
    int r = z;
    return new Vector2(
      size * 1.5f * q,
      - size * (SQRT_3 / 2 * q + SQRT_3 * r)
    );
  }

  public override string ToString() {
    return $"HexCoord({x}, {y}, {z})";
  }

  public static HexCoord FromWorldPosition(Vector2 position, float size) {
    float x = (2.0f / 3) * position.x / size;
    float z = (-1.0f / 3 * position.x + SQRT_3 / 3.0f * (-position.y)) / size;
    float y = -x - z;

    int rx = (int) Mathf.Round(x);
    int ry = (int) Mathf.Round(y);
    int rz = (int) Mathf.Round(z);

    float xDiff = Mathf.Abs(rx - x);
    float yDiff = Mathf.Abs(ry - y);
    float zDiff = Mathf.Abs(rz - z);

    if ((xDiff > yDiff) && (xDiff > zDiff)) {
      rx = -ry - rz;
    } else if (yDiff > zDiff) {
      ry = -rx - rz;
    } else {
      rz = -rx - ry;
    }

    return new HexCoord(rx, ry, rz);
  }

  public HexCoord[] Neighbors {
    get {
      return new HexCoord[] {
        new HexCoord(x + 1, y - 1, z),
          new HexCoord(x + 1, y, z - 1),
          new HexCoord(x - 1, y + 1, z),
          new HexCoord(x, y + 1, z - 1),
          new HexCoord(x - 1, y, z + 1),
          new HexCoord(x, y - 1, z + 1)
      };
    }
  }
}
