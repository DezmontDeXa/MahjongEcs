using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "Configs/Enemy/AngelConfig")]
    public class AngelConfig : EnemyConfig
    {
        public DiceMono DicePrefab;
        public int RestoreDicesCount = 1;
    }
}
