using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropInterno : MonoBehaviour, IBeginDragHandler, IDragHandler {
    private Canvas parentCanvas;
    private Vector3 moveOffset;

    void Start() {
        // get the "nearest Canvas"
        parentCanvas = GetComponentsInParent<Canvas>()[0];
        // you could get the Camera of this Canvas here as well and like @ina suggests add a null check for that and otherwise use Camera.Main
    }
    public void OnBeginDrag(PointerEventData eventData) {

        //For Offset Position so the object won't "jump" to the mouse/touch position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, eventData.position, parentCanvas.worldCamera, out Vector2 pos);
        moveOffset = transform.position - parentCanvas.transform.TransformPoint(pos);

    }

    public void OnDrag(PointerEventData eventData) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, eventData.position, parentCanvas.worldCamera, out Vector2 pos);
        transform.position = parentCanvas.transform.TransformPoint(pos) + moveOffset;
    }
}