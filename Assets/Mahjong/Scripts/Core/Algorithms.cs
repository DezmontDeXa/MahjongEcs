using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public static class Algorithms
{
    public class Neighbor
    {
        public Vector3 Position { get; }
        public Neighbor Left => new Neighbor(new Vector3(Position.x - _step, Position.y, Position.z));
        public Neighbor TopLeft => new Neighbor(new Vector3(Position.x - _step, Position.y - _step, Position.z));
        public Neighbor Top => new Neighbor(new Vector3(Position.x, Position.y - _step, Position.z));
        public Neighbor TopRight => new Neighbor(new Vector3(Position.x + _step, Position.y - _step, Position.z));
        public Neighbor Right => new Neighbor(new Vector3(Position.x + _step, Position.y, Position.z));
        public Neighbor BottomRight => new Neighbor(new Vector3(Position.x + _step, Position.y + _step, Position.z));
        public Neighbor Bottom => new Neighbor(new Vector3(Position.x, Position.y + _step, Position.z));
        public Neighbor BottomLeft => new Neighbor(new Vector3(Position.x - _step, Position.y + _step, Position.z));

        public Neighbor Up => new Neighbor(new Vector3(Position.x, Position.y, Position.z + 1));
        public Neighbor Down => new Neighbor(new Vector3(Position.x, Position.y, Position.z - 1));

        private List<Neighbor> _neighbors;
        private readonly float _step;

        public Neighbor(Vector3 position, float step = 0.5f)
        {
            Position = position;
            _step = step;
        }

        public bool Exist(Vector3 position, int stepsCount = 1)
        {
            if (Equals(position)) return false;

            if (_neighbors == null)
                _neighbors = new List<Neighbor>()
            {
                TopLeft, Top, TopRight,
                Left, this, Right,
                BottomLeft, Bottom, BottomRight
            };

            foreach (var neighbor in _neighbors)
                if (neighbor.Position.Equals(position)) return true;

            stepsCount--;
            if (stepsCount > 0)
                foreach (var neighbor in _neighbors)
                    if (neighbor.Exist(position, stepsCount)) return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                var vector = (Vector3)obj;
                return vector.x == Position.x && vector.y == Position.y && vector.z == Position.z;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return Position.ToString();
        }

        public List<Vector3> SelectExists(IEnumerable<Vector3> existPositions, int stepsCount = 1, List<Vector3> result = null)
        {
            if (result == null)
                result = new List<Vector3>();

            if (_neighbors == null)
                _neighbors = new List<Neighbor>()
            {
                TopLeft, Top, TopRight,
                Left, this, Right,
                BottomLeft, Bottom, BottomRight
            };

            foreach (var existPoint in existPositions)
            {
                foreach (var neighbor in _neighbors)
                    if (neighbor.Position.Equals(existPoint))
                        result.Add(existPoint);
            }

            stepsCount--;
            if (stepsCount > 0)
                foreach (var neighbor in _neighbors)
                    foreach (var point in neighbor.SelectExists(existPositions, stepsCount, result))
                        result.Add(point);

            return result.Distinct().ToList();
        }
    }

    /// <summary>
    /// Check position Can Select
    /// </summary>
    /// <param name="position"></param>
    /// <param name="existPositions"></param>
    /// <returns></returns>
    public static bool PositionCanSelect(Vector3 position, IEnumerable<Vector3> existPositions)
    {
        var neighbor = new Neighbor(position);
        var existPoints = neighbor.SelectExists(existPositions, 2);

        // Has any on above level
        var neighborUp = neighbor.Up;
        foreach (var existPoint in existPositions)
        {
            if (neighborUp.Equals(existPoint))
                return false;

            if (neighborUp.Exist(existPoint, 1))
                return false;
        }

        // Has any left and any right on that level
        var leftNeighbors = new List<Neighbor>() { neighbor.Left.Left, neighbor.Left.Left.Top, neighbor.Left.Left.Bottom };
        var rightNeighbors = new List<Neighbor>() { neighbor.Right.Right, neighbor.Right.Right.Top, neighbor.Right.Right.Bottom };


        foreach (var leftNeighbor in leftNeighbors)
        {
            foreach (var existPoint in existPoints)
            {
                if (leftNeighbor.Equals(existPoint))
                {
                    foreach (var rightNeighbor in rightNeighbors)
                    {
                        foreach (var existPoint1 in existPoints)
                        {
                            if (existPoint1.Equals(rightNeighbor.Position))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return true;
    }

    public static bool PositionCanPlace(Vector3 position, List<Vector3> points)
    {
        if (points.Any(p => p.x == position.x && p.y == position.y && p.z == position.z)) return false;

        var neighbor = new Neighbor(position);
        foreach (var point in points)
        {
            if (neighbor.Exist(point))
                return false;
        }
        return true;
    }

    public static bool HasIntersection<T>(IEnumerable<T> first, IEnumerable<T> second)
    {
        foreach (var f in first)
        {
            foreach (var s in second)
            {
                if (f.Equals(s))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Generate random point with step 0.5f
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetRandomGridPoint(Vector3 _size)
    {
        var x = RoundToFraction(Random.Range(0, _size.x - 1), 0.5f);
        var y = RoundToFraction(Random.Range(0, _size.y - 1), 0.5f);
        var z = RoundToFraction(Random.Range(0, _size.z - 1), 1f);
        return new Vector3(x, y, z);

    }

    /// <summary>
    /// Round value to fraction. Example  Round(3.3, 0.5) = 3.5
    /// </summary>
    /// <param name="value"></param>
    /// <param name="fraction"></param>
    /// <returns></returns>
    public static float RoundToFraction(float value, float fraction)
    {
        return Mathf.Round(value / fraction) * fraction;
    }

}
