using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using static Algorithms;
using System.Linq;

namespace DDX
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(RandomFigureGeneratorSystem))]
    public sealed class RandomFigureGeneratorSystem : SimpleUpdateSystem<RequestGenerateFigureEvent>
    {
        [SerializeField] private RandomFigureGeneratorSettings _settings;
        private GameObject _dicePrefab => _settings.DicePrefab;
        private Vector3Int _size => _settings.Size;
        private float _fillPercentage => _settings.FillPercentage;
        private RectTransform _dicesContainer;

        public override void OnUpdate(float deltaTime)
        {
            _dicesContainer = World.Filter.With<DicesContainerRef>().First().GetComponent<DicesContainerRef>().Container;
            base.OnUpdate(deltaTime);
        }

        protected override void Process(Entity entity, ref RequestGenerateFigureEvent component, in float deltaTime)
        {
            var count = ((int)(_size.x * _size.y * _size.z * _fillPercentage / 3)) * 3;
            var points = GetRandomPoints(count);
            if (points.Count != count)
                while (points.Count % 3 != 0)
                    points.Remove(points.First());

            points = DropFlyingPoints(points);

            foreach (var point in points)
            {
                var dice = Instantiate(_dicePrefab, _dicesContainer);
                var gridPos = dice.GetComponent<InGridPositionMono>();
                gridPos.GetData().Position = point;
                gridPos.Entity.AddComponent<GeneratedDiceTag>();
            }

            World.CreateEntity().AddComponent<CanSelectChangedEvent>();
            World.CreateEntity().AddComponent<FigureGeneratedEvent>();
        }

        private List<Vector3> GetRandomPoints(int count)
        {
            var allPoints = GenerateAllGridPoints();

            var points = new List<Vector3>();
            for (int i = 0; i < count; i++)
            {
                if (allPoints.Count == 0)
                    return points;

                var randomIndex = UnityEngine.Random.Range(0, allPoints.Count);
                var randomPoint = allPoints[randomIndex];
                points.Add(randomPoint);

                // Remove points where we can't place dice
                allPoints.RemoveAt(randomIndex);
                var neighbor = new Neighbor(randomPoint);
                foreach (var pointToRemove in neighbor.SelectExists(allPoints, 1))
                    allPoints.RemoveAll(p => p.x == pointToRemove.x && p.y == pointToRemove.y && p.z == pointToRemove.z);
            }

            return points;
        }

        private static void TestCheckError(List<Vector3> points)
        {
            foreach (var p in points)
            {
                int cnt = 0;
                foreach (var p1 in points)
                {
                    if (p.x == p1.x && p.y == p1.y && p.z == p1.z)
                    {
                        if (cnt > 0)
                            throw new InvalidImplementationException("Generator generate same dices in one poistion");
                        cnt++;
                    }
                }
            }
        }

        private List<Vector3> GenerateAllGridPoints()
        {
            var allPoints = new List<Vector3>((_size.x * 2) * (_size.y * 2) * _size.z);
            for (float x = 0; x < _size.x; x += 0.5f)
            {
                for (float y = 0; y < _size.y; y += 0.5f)
                {
                    for (float z = 0; z < _size.z; z++)
                    {
                        allPoints.Add(new Vector3(x, y, z));
                    }
                }
            }
            return allPoints;
        }

        private List<Vector3> DropFlyingPoints(List<Vector3> points)
        {
            var result = new List<Vector3>();
            int dropsCount = 0;
            foreach (var point in points)
            {
                var level = (int)point.z - 1;
                if (level >= 0)
                {
                    var neighborsBelow = new Algorithms.Neighbor(point).Down;
                    var hasBelow = false;
                    foreach (var existPoint in points)
                    {
                        if (neighborsBelow.Equals(existPoint) || neighborsBelow.Exist(existPoint))
                        {
                            hasBelow = true;
                            break;
                        }
                    }

                    if (!hasBelow)
                    {
                        result.Add(new Vector3(point.x, point.y, point.z - 1));
                        dropsCount++;
                        continue;
                    }
                }
                result.Add(point);
            }

            if (dropsCount > 0)
                return DropFlyingPoints(result);

            return result;
        }
    }
}