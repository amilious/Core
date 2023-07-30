
using UnityEngine;
using System.Collections;
using Amilious.Core.Extensions;
using UnityEngine.EventSystems;
using Amilious.Core.UI.Component;

namespace Amilious.Core.UI.Tooltip {
    
    
    [DisallowMultipleComponent, AddComponentMenu("Amilious/UI/Tooltip/Tooltip Provider")]
    public class TooltipProvider : AmiliousBehavior, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField, Min(0)] private float tooltipDelay = .75f;
        [SerializeField, Multiline] private string header;
        [SerializeField, Multiline] private string text;
        
        public bool HasController { get; private set; }
        
        public TooltipController TooltipController { get; private set; }

        private YieldInstruction _wait;
        private Coroutine _showCoroutine;
        private UIController _uiController;
        
        private void Awake() {
            _uiController = FindObjectOfType<UIController>();
            if(_uiController != null) return;
            Debug.LogWarningFormat("The tooltip provider missing parent with UIController': {0}", transform.GetScenePath());
        }

        private void Start() {
            TooltipController = _uiController.TooltipController;
            HasController = TooltipController != null;
            if(!HasController)
                Debug.LogWarningFormat("The tooltip provider missing tooltip controller {0}", transform.GetScenePath());
            _wait = new WaitForSeconds(tooltipDelay);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(!HasController) return;
            if(_showCoroutine != null) StopCoroutine(_showCoroutine);
            _showCoroutine = StartCoroutine(ShowTooltip());
        }

        public void OnPointerExit(PointerEventData eventData) {
            if(!HasController) return;
            StopCoroutine(_showCoroutine);
            TooltipController.Hide();
        }

        protected virtual IEnumerator ShowTooltip() {
            yield return _wait;
            TooltipController.Show(text, header);
        }
        
    }
}