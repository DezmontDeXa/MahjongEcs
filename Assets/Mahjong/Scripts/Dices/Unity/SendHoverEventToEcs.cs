using Scellecs.Morpeh;
using Scellecs.Morpeh.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DDX
{
    public class SendHoverEventToEcs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private EntityRefMono _entityRef;

        private void Awake()
        {
            _entityRef = GetComponent<EntityRefMono>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _entityRef.GetData().Entity.AddComponent<DicePointerEnterEvent>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _entityRef.GetData().Entity.AddOrGet<DicePointerExitEvent>();
        }
    }
}
