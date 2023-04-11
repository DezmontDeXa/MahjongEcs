using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh.Helpers;
using Scellecs.Morpeh;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace DDX
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HandLenghtSystem))]
	public sealed class HandLenghtSystem : SimpleUpdateSystem<HandCell>
	{
		[SerializeField] private HandSettings _settings;

        protected override void Process(Entity entity, ref HandCell cell, in float deltaTime)
        {         
            cell.GameObject.SetActive(cell.InHandIndex < _settings.Lenght);            
        }
    }
}