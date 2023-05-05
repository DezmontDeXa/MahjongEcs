using DDX;
using Scellecs.Morpeh;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public static partial class Algorithms
{

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

    public static Collider2D[] GetCollidersUnderPoint(Vector3 point)
    {
        var hits = Physics2D.RaycastAll(
            point,
            Vector2.zero);

        if (hits.Length == 0)
            return new Collider2D[0];

        var colliders = hits.Where(x => x.collider != null).Select(x => x.collider).Distinct().ToArray();

        return colliders;
    }

    public static Entity GetNearestEntity(Vector3 point)
    {
        var colliders = GetCollidersUnderPoint(point);
        if (colliders.Length == 0) return null;
        var nearestCollider = colliders.OrderBy(h => CalcDistance(h, point)).First();
        var entity = nearestCollider.transform.gameObject.GetComponent<EntityRefMono>().Entity;

        return entity;
    }

    private static float CalcDistance(Collider2D collider, Vector3 worldMousePosition)
    {
        return Vector2.Distance(collider.bounds.center, worldMousePosition);
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

    public static Entity GetNearestEntity(Vector3 worldMousePosition, IEnumerable<Collider2D> avaliableColliders)
    {
        var nearestCollider = avaliableColliders.OrderBy(h => CalcDistance(h, worldMousePosition)).First();
        var entity = nearestCollider.transform.gameObject.GetComponent<EntityRefMono>().Entity;

        return entity;
    }

    public static bool CompareVector3(Vector3 x, Vector3 y)
    {
        return x.x == y.x && x.y == y.y && x.z == y.z;
    }

    public class Vector3Comparer : IEqualityComparer<Vector3>
    {
        public bool Equals(Vector3 x, Vector3 y)
        {
            return x.x == y.x && x.y == y.y && x.z == y.z;
        }

        public int GetHashCode(Vector3 obj)
        {
            return obj.GetHashCode();
        }
    }
}
