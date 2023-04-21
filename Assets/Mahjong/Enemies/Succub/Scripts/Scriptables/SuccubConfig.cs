using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "Configs/Enemy/SuccubConfig")]
    public class SuccubConfig : EnemyConfig
    {
        public GameObject SuccubDiceEffectPrefab;
        public int CountOfBlockingPerAttack = 1;
    }
}
