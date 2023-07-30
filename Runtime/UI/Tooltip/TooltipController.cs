
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Input;
using Amilious.Core.Extensions;
using Amilious.Core.UI.Component;
using Amilious.Core.UI.Tooltip.ShowHide;

namespace Amilious.Core.UI.Tooltip {
    
    [AddComponentMenu("Amilious/UI/Tooltip/Tooltip Controller")]
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class TooltipController : AmiliousBehavior {

        [SerializeField] private UIController uIController;
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField,Min(100)] private float wrapWidth = 500;
        [SerializeField] private Vector2 mouseOffset = new Vector2(25,-25);
        
        private CanvasGroup _canvasGroup;
        private Vector2 _lastPosition = Vector3.zero;
        private RectTransform _rectTransform;
        private AbstractTooltipShowHide _showHide;
        private Vector2 _screenSize;

        public CanvasGroup CanvasGroup => this.GetCacheComponent(ref _canvasGroup);
        
        public RectTransform RectTransform => this.GetCacheComponent(ref _rectTransform);
        
        private void Awake() {
            //tooltips should not block raycasts
            _showHide = GetComponent<AbstractTooltipShowHide>();
            uIController ??= GetComponent<UIController>();
            uIController ??= FindObjectOfType<UIController>();
            _screenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Start() {
            CanvasGroup.blocksRaycasts = false;
            layoutElement.preferredWidth = wrapWidth;
        }

        private void Update() {
            if(CanvasGroup.alpha == 0) return; //do not need to follow the mouse
            var pointer = InputHelper.PointerPosition + mouseOffset;
            if(_lastPosition == pointer) return;
            _lastPosition = pointer;
            var toolTipSize = RectTransform.sizeDelta;
            if(uIController != null) { toolTipSize *= uIController.Canvas.scaleFactor; }
            var edge = pointer;
            edge.x += toolTipSize.x;
            edge.y -= toolTipSize.y;
            if(edge.x < _screenSize.x && edge.y >0) {
                RectTransform.SetCornerPosition(MoveSource.TopLeft,pointer);
                return;
            }
            pointer -= mouseOffset;
            if(edge.x > _screenSize.x && edge.y <0)
                RectTransform.SetCornerPosition(MoveSource.BottomRight,pointer);
            else if(edge.x > _screenSize.x) RectTransform.SetCornerPosition(MoveSource.TopRight,pointer);
            else RectTransform.SetCornerPosition(MoveSource.BottomLeft,pointer);
        }

        public void Show(string text, string header="") {
            if(headerText != null&&!string.IsNullOrEmpty(header)) {
                headerText.gameObject.SetActive(true);
                headerText.SetText(header);
            }else { headerText.gameObject.SetActive(false); }
            if(bodyText!=null) bodyText.SetText(text);
            FixTooltip();
            if(_showHide != null) _showHide.OnShow(CanvasGroup);
            else CanvasGroup.alpha = 1f;
        }

        public void Hide() {
            if(_showHide != null) _showHide.OnHide(CanvasGroup);
            else CanvasGroup.alpha = 0f;
        }

        private void FixTooltip() {
            if(headerText!=null) headerText.ForceMeshUpdate();
            if(bodyText!=null) bodyText.ForceMeshUpdate();
            var headerWidth = (headerText==null)?0f:headerText.GetPreferredValues().x;
            var bodyWidth = (bodyText==null)?0f:bodyText.GetPreferredValues().x;
            layoutElement.enabled = !(headerWidth < wrapWidth-10 && bodyWidth < wrapWidth-10);
        }
        
    }
}