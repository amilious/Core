
using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.Progress {
    
    /// <summary>
    /// This class can be used to rotate an object based on a progress value.
    /// </summary>
    [ExecuteAlways, RequireComponent(typeof(ProgressController))]
    [AddComponentMenu(AmiliousCore.PROGRESS_CONTEXT_MENU+"Progress Rotator",1)]
    public class ProgressRotator : AmiliousBehavior {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private bool negateDirection;
        [SerializeField] private RectTransform rotationObject;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private ProgressController _progressController;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public ProgressController ProgressController => this.GetCacheComponent(ref _progressController);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void OnEnable() => ProgressController.OnProgress += OnProgressUpdated;

        private void OnDisable() => ProgressController.OnProgress -= OnProgressUpdated;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        private void OnProgressUpdated(int current, int min, int max, float percentage) {
            rotationObject.localRotation = Quaternion.Euler(new Vector3(0f, 0f, GetRotation(percentage)));
        }

        private float GetRotation(float progress) => negateDirection ? progress * 360 : -progress * 360;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}