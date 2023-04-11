using UnityEngine;

[CreateAssetMenu(fileName = "Random Figure Generator Settings")]
public class RandomFigureGeneratorSettings : ScriptableObject
{
    public GameObject DicePrefab;
    public Vector3Int Size;
    [Range(0, 1f)] public float FillPercentage;
}
