using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace DDX
{
    [System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct Grid : IComponent 
	{
        public Transform Transform;
        public RectTransform StartPoint;
        public Vector2 Step;
        public Vector2 LevelOffset;
    }
}