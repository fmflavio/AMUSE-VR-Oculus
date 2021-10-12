using UnityEngine;
using UnityEngine.EventSystems;
using Wacki;
using System.Linq;

public class DragDropOculus : MonoBehaviour, IPointerDownHandler, IDragHandler {
    private Vector3 mOffset, temp;
    public GameObject window;

    public void OnDrag(PointerEventData eventData) {
        //transform.position = eventData.pointerCurrentRaycast.worldPosition + mOffset;
        temp = eventData.pointerCurrentRaycast.worldNormal + mOffset;
    }

    public void OnPointerDown(PointerEventData eventData) {
        //mOffset = gameObject.transform.position - eventData.pointerCurrentRaycast.worldPosition;
        window.transform.position = temp;
    }
}