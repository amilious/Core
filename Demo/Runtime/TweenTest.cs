
using System.Net.Mime;
using UnityEngine;
using Amilious.Core;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using Amilious.Tweening;
using Amilious.Tweening.Easing;
using Amilious.Tweening.Tweens;
using UnityEngine.UI;
using Tween = Amilious.Tweening.Tween;

namespace Demo.Runtime {
    
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class TweenTest : AmiliousBehavior {

        
        public bool relative = true;
        public TweenEasing easing = TweenEasing.Linear;
        public MoveSource positionType = MoveSource.Center;

        public float rotation;
        public MoveSource rotationPoint;
        [AmiVector("Duration", "Delay")] public Vector2 tweenTime = new Vector2(2, .5f);
        [AmiVector("X", "Y")]public Vector2 targetPosition = Vector2.up * 100;
        public Color color1 = Color.black;
        public Color color2 = Color.white;
        private RectTransform _rectTransform;
        private Image _image;
        private TweenGroup _tweenGroup;
        private bool _started;
        private bool _paused;
        private int _currentColor = 0;
        private float _canvasGoal = 0;

        private RectTransform RectTransform => this.GetCacheComponent(ref _rectTransform);
        private Image Image => this.GetCacheComponent(ref _image);
        
        [AmiButton("Start Tween")]
        [AmiHideIf(nameof(_started))]
        private void StartTween() {
            _tweenGroup = Tween.CreateGroup(tweenTime.x, false, easing).OnStart(OnStarted).OnStop(OnStop)
                .OnResume(OnResume).OnPaused(OnPaused).OnComplete(OnCompleted);
            Tween.MoveRect(_tweenGroup, gameObject, targetPosition, relative: relative, moveSource:positionType);
            Tween.Color(_tweenGroup, Image, _currentColor==0?color1:color2);
            //Tween.Fade(_tweenGroup, gameObject, canvasGoal);
            Tween.RotateRect(_tweenGroup, RectTransform, rotation, positionType: rotationPoint,relative:false);
            _tweenGroup.Start(tweenTime.y);
        }

        [AmiButton("Pause Tween")]
        [AmiShowIf(nameof(_started))]
        [AmiHideIf(nameof(_paused))]
        private void PauseTween() {
            if(_tweenGroup.State == TweenState.Completed) return;
            _tweenGroup.Pause();
        }

        [AmiButton("Resume Tween")]
        [AmiShowIf(nameof(_paused))]
        private void ResumeTween() {
            if(_tweenGroup.State == TweenState.Completed) return;
            _tweenGroup.Resume();
        }

        [AmiButton("Stop Tween")]
        [AmiShowIf(nameof(_started))]
        private void StopTween() {
            if(_tweenGroup.State == TweenState.Completed) return;
            _tweenGroup.Stop();
        }

        private void OnCompleted() {
            _started = false;
            if(relative) targetPosition *= -1;
            if(_currentColor == 0) _currentColor = 1;
            else _currentColor = 0;
            rotation *= -1;
            
            _canvasGoal=_canvasGoal == 0?1:0;
            Debug.Log("The tween completed!");
        }

        private void OnStarted() {
            _started = true;
            Debug.Log("The tween started!");
        }

        private void OnPaused() {
            _paused = true;
            Debug.Log("The tween has paused!");
        }

        private void OnResume() {
            _paused = false;
            Debug.Log("The tween has resumed!");
        }

        private void OnStop() {
            _started = false;
            Debug.Log("The tween has stopped!");
        }
        
    }
    
}