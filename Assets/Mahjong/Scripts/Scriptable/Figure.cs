using System.Collections.Generic;
using UnityEngine;

namespace DDX
{
    [CreateAssetMenu(fileName =  "Figure")]
    public class Figure : ScriptableObject
    {
        public string Name;
        public List<Vector3> Points;
    }
}
