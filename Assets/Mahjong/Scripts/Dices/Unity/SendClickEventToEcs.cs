using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDX
{
    public class SendClickEventToEcs : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.GetComponent<EntityRefMono>().GetData().Entity.AddComponent<DiceClickedEvent>();
        }
    }
}
