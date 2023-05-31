
using UnityEngine;
using UnityEngine.UI;
using Amilious.Core.Attributes;

namespace Amilious.Core.UI.Progress {
    
    [ExecuteAlways, RequireComponent(typeof(Image))]
    [AddComponentMenu(AmiliousCore.PROGRESS_CONTEXT_MENU+"Progress Controller",0)]
    public class ProgressController : AmiliousBehavior {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////

        // ReSharper disable MemberCanBePrivate.Global
        protected const string VALUES_TAB = "Values";
        protected const string VISUALS_TAB = "Visuals";
        protected const string REFERENCES_TAB = "References";
        // ReSharper restore MemberCanBePrivate.Global
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, AmiTab(VALUES_TAB)] private int minimum = 0;
        [SerializeField, AmiTab(VALUES_TAB)] private int maximum = 100;
        [SerializeField, AmiTab(VALUES_TAB)] private int current = 0;
        [SerializeField, AmiTab(VALUES_TAB)] private float percentage = 0f;
        [SerializeField, AmiTab(VISUALS_TAB), AmiColor] private Color backgroundColor;
        [AmiEnableIf(nameof(fill)),SerializeField, AmiTab(VISUALS_TAB), AmiColor] private Color fillColor;
        [SerializeField, AmiTab(REFERENCES_TAB)] private Image mask;
        [SerializeField, AmiTab(REFERENCES_TAB)] private Image fill;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="ProgressController.OnProgress"/> event.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="percentage">The current percentage.</param>
        public delegate void OnProgressDelegate(int current, int min, int max, float percentage);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        public event OnProgressDelegate OnProgress;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private Image _background;
        private int _previousMin, _previousMax, _previousCurrent;
        private float _previousPercentage;
        private bool _updated;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get or set the minimum value.
        /// </summary>
        public int Minimum {
            get => minimum;
            set {
                if(value == minimum) return;
                minimum = value;
                RecalculatePercentage();
            }
        }

        /// <summary>
        /// This property is used to get or set the maximum value.
        /// </summary>
        public int Maximum {
            get => maximum;
            set {
                if(value == maximum) return;
                maximum = value;
                RecalculatePercentage();
            }
        }

        /// <summary>
        /// This property is used to get or set the current value.
        /// </summary>
        public int Current {
            get => current;
            set {
                if(current == value) return;
                current = value;
                RecalculatePercentage();
            }
        }

        /// <summary>
        /// This property is used to get or set the current percentage.
        /// </summary>
        public float Percentage {
            get => percentage;
            set {
                if(percentage < 0) percentage = 0;
                if(percentage > 1) percentage = 1;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if(percentage == value) return;
                percentage = value;
                _previousPercentage = value;
                current = Mathf.FloorToInt(Mathf.InverseLerp(minimum, maximum, value));
                if(mask != null) mask.fillAmount = percentage;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region MonoBehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when the object is first created.
        /// </summary>
        private void Awake() {
            _background = GetComponent<Image>();
            RecalculatePercentage();
            if(!Application.isEditor) return;
            _previousMax = maximum;
            _previousMin = minimum;
            _previousCurrent = current;
            _previousPercentage = percentage;
        }

        /// <summary>
        /// This method is called every frame.
        /// </summary>
        private void Update() {
            if(!_updated) return;
            OnProgress?.Invoke(current,minimum,maximum,percentage);
            _updated = false;
        }

        /// <summary>
        /// This method is called when the values in the inspector are updated.
        /// </summary>
        private void OnValidate() {
            //the current value was changed.
            if(_previousCurrent != current) {
                if(current < minimum) current = minimum;
                if(current > maximum) current = maximum;
                percentage = (current - minimum) / ((float)maximum - minimum);
                _updated = true;
            }
            //the percentage was changed.
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if(percentage != _previousPercentage) {
                if(percentage < 0) percentage = 0;
                if(percentage > 1) percentage = 1;
                current = Mathf.FloorToInt(Mathf.InverseLerp(minimum, maximum, percentage));
                _updated = true;
            }
            //the maximum was changed.
            if(maximum != _previousMax) {
                if(minimum > maximum) minimum = maximum;
                if(current > maximum) current = maximum;
                percentage = (current - minimum) / ((float)maximum - minimum);
                _updated = true;
            }
            //the minimum was changed.
            if(minimum != _previousMin) {
                if(maximum < minimum) maximum = minimum;
                if(current < minimum) current = minimum;
                percentage = (current - minimum) / ((float)maximum - minimum);
                _updated = true;
            }
            //update values
            _previousMax = maximum;
            _previousMin = minimum;
            _previousPercentage = percentage;
            _previousCurrent = current;
            if(!_updated) return;
            if(mask != null) mask.fillAmount = percentage;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to recalculate values.
        /// </summary>
        private void RecalculatePercentage() {
            if(maximum < minimum) maximum = minimum;
            if(current < minimum) current = minimum;
            if(current > maximum) current = maximum;
            percentage = (current - minimum) / ((float)maximum - minimum);
            if(mask != null) mask.fillAmount = percentage;
            _updated = true;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}
