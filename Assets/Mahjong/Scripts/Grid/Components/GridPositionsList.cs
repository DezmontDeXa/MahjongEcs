using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

namespace DDX
{
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct GridPositionsList : IComponent 
	{
		public Dictionary<Entity, InGridPosition> AllPositions;
        public Dictionary<Entity, InGridPosition> ConstructorPlacedPositions;
    }
}