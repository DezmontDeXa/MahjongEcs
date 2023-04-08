using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;

namespace DDX
{
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct DiceImageRef : IComponent 
	{
		public Image BackImage;
		public Image Image;
	}
}