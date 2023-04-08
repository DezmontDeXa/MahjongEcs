using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName = "DiceData")]
    public class DiceData : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private DiceType _type;

        public int Id => _id;
        public Sprite Sprite => _sprite;
        public DiceType Type => _type;
    }

    public enum DiceType
    {
        Fruits,
        Sweets,
        Vegetables
    }
}
