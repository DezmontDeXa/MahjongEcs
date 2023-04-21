using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "Configs/Enemy/DruidConfig")]
    public class DruidConfig : EnemyConfig
    {
        public int RootsPerAttack = 1;
        public int UnRootsPerAssembly = 1;
        public GameObject RootEffectPrefab;
    }
}
