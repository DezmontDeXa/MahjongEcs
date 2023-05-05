using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class Algorithms
{
    public class Neighbor
    {
        public Vector3 Position { get; }
        public virtual Neighbor Left => new Neighbor(new Vector3(Position.x - _step, Position.y, Position.z));
        public virtual Neighbor TopLeft => new Neighbor(new Vector3(Position.x - _step, Position.y - _step, Position.z));
        public virtual Neighbor Top => new Neighbor(new Vector3(Position.x, Position.y - _step, Position.z));
        public virtual Neighbor TopRight => new Neighbor(new Vector3(Position.x + _step, Position.y - _step, Position.z));
        public virtual Neighbor Right => new Neighbor(new Vector3(Position.x + _step, Position.y, Position.z));
        public virtual Neighbor BottomRight => new Neighbor(new Vector3(Position.x + _step, Position.y + _step, Position.z));
        public virtual Neighbor Bottom => new Neighbor(new Vector3(Position.x, Position.y + _step, Position.z));
        public virtual Neighbor BottomLeft => new Neighbor(new Vector3(Position.x - _step, Position.y + _step, Position.z));

        public virtual Neighbor Up => new Neighbor(new Vector3(Position.x, Position.y, Position.z + 1));
        public virtual Neighbor Down => new Neighbor(new Vector3(Position.x, Position.y, Position.z - 1));

        private List<Neighbor> _neighbors;
        protected readonly float _step;

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

            GetList();

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

        public List<Neighbor> GetList(bool returnThis = true)
        {
            if (_neighbors == null)
                _neighbors = new List<Neighbor>()
            {
                TopLeft, Top, TopRight,
                Left, returnThis?this:null, Right,
                BottomLeft, Bottom, BottomRight
            };
            return _neighbors;
        }
    }
}
