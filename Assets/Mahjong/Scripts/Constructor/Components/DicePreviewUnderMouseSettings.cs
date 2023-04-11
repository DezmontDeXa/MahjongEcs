using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace DDX
{
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct DicePreviewUnderMouseSettings : IComponent 
	{
		public RectTransform GridTransform;
		public RectTransform GridStartPoint;
        public InGridPositionMono DicePrefab;
    }
}