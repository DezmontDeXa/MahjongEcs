using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "AngelConfig")]
    public class AngelConfig : ScriptableObject
    {
        public DiceMono DicePrefab;
        public int StepsToAttack = 3;
        public int RestoreDicesCount = 1;
    }
}
