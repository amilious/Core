using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.Core.UI.Tooltip.ShowHide {
    
    [RequireComponent(typeof(TooltipController))]
    public abstract class AbstractTooltipShowHide : AmiliousBehavior {
        
        private TooltipController _tooltipController;

        public TooltipController TooltipController => this.GetCacheComponent(ref _tooltipController);
        
        public abstract void OnShow(CanvasGroup canvasGroup);

        public abstract void OnHide(CanvasGroup canvasGroup);
        
    }
}