using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class GameObjectRefMono : MonoProvider<GameObjectRef> 
	{
        [SerializeField] private bool _useThisGameObject = true;

        protected override void Initialize()
        {
            base.Initialize();
            if(_useThisGameObject)
                GetData().GameObject = gameObject;
        }
    }
}