using Scellecs.Morpeh;
using UnityEngine;

namespace DDX
{
    public class MenuController : MonoBehaviour
    {
        public void GenerateRandomGrid()
        {
            GetComponent<EntityRefMono>().Entity.AddComponent<RequestGenerateFigureEvent>();
        }

        public void SaveGrid()
        {

        }

        public void SetGridName(string gridName)
        {

        }

        public void LoadGrid()
        {

        }

        public void SelectExistGrid()
        {

        }


    }
}
