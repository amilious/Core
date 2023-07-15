
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Amilious.Core.Extensions;
using Amilious.Core.UI.Cursors;

[RequireComponent(typeof(RectTransform))]
public class CursorCalibrator : MonoBehaviour, IPointerDownHandler {

    [SerializeField] private CursorInfo cursorInfo;
    [SerializeField] private TMP_Text results;
    [SerializeField] private Image hitImage;
    
    private RectTransform _rectTransform;
    
    public RectTransform RectTransform => this.GetCacheComponent(ref _rectTransform);

    private void Start() {
        if(cursorInfo == null) cursorInfo = CursorController.CurrentCursor;
        CursorController.SetCursor(cursorInfo);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(CursorController.Instance == null) return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, eventData.position, 
            eventData.pressEventCamera, out var worldPoint)) {
            var clickP = (Vector2)(RectTransform.position - worldPoint);
            var cHotSpot = CursorController.CurrentCursor.HotSpot;
            var newP = new Vector2(clickP.x+cHotSpot.x,cHotSpot.y-clickP.y);
            hitImage.gameObject.SetActive(true);
            hitImage.transform.localPosition = -clickP;
            //newHotSpot.x = Mathf.Clamp01(newHotSpot.x);
            //newHotSpot.y = Mathf.Clamp01(newHotSpot.y);
            var result = "Results:\n";
            result += $"Current Hot Spot: {cHotSpot}\n";
            //result += $"Current Point: {oldP}\n";
            result += $"Click Point: {clickP}\n";
            //result += $"New Point: {newP}\n";
            result += $"New HotSpot: {newP}";
            results.SetText(result);
        }
        
    }

    #if UNITY_EDITOR
    
    private void OnValidate() {
        if(CursorController.Instance == null) return;
        CursorController.SetCursor(cursorInfo);

    }
    
    #endif
    
}
