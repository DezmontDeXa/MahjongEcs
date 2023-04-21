using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "DruidConfig")]
    public class DruidConfig : ScriptableObject
    {
        public int StepsToAttack = 3;
        public int RootsPerAttack = 1;
        public int UnRootsPerAssembly = 1;
        public GameObject RootEffectPrefab;
    }
}
