/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious.comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using UnityEngine;
using Amilious.Core.Extensions;
using UnityEngine.EventSystems;
using Amilious.Core.Attributes;
using System.Collections.Generic;

namespace Amilious.Core.UI.Text {
    
    [AmiHelpBox("This component is used as a context menu for link actions!")]
    public class LinkActionMenu : AmiliousBehavior, IPointerExitHandler {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private RectTransform bindingRect;
        [SerializeField] private GameObject actionPrefab;
        [SerializeField] private Color mouseOverColor = Color.white;
        [SerializeField] private Color mouseNotOverColor = Color.gray;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private RectTransform _rectTransform;
        private readonly Queue<LinkActionItem> _actionItemPool = new Queue<LinkActionItem>();
        private readonly List<LinkActionItem> _currentActions = new List<LinkActionItem>();

        public Color MouseNotOverColor {
            get => mouseNotOverColor;
            set => mouseNotOverColor = value;
        }

        public Color MouseOverColor {
            get => mouseOverColor;
            set => mouseOverColor = value;
        }

        public RectTransform RectTransform {
            get {
                if(_rectTransform != null) return _rectTransform;
                _rectTransform = (RectTransform)transform;
                return _rectTransform;
            }
        }

        public RectTransform BindingRect {
            get {
                if(bindingRect != null) return bindingRect;
                bindingRect = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
                return BindingRect;
            }
        }

        public void AddAction(string actionName, Action action) {
            var actionItem = (_actionItemPool.Count>0)?
                _actionItemPool.Dequeue():new LinkActionItem(actionPrefab,this);
            _currentActions.Add(actionItem);
            actionItem.SetUpAction(actionName, action);
        }

        public void Hide() {
            foreach(var action in _currentActions) {
                action.Hide();
                _actionItemPool.Enqueue(action);
            }
            _currentActions.Clear();
            gameObject.SetActive(false);
        }

        public void Show(Vector3 position) {
            gameObject.SetActive(true);
            RectTransform.TryClampUIToScreen(BindingRect, position);
            Canvas.ForceUpdateCanvases();
            RectTransform.TryClampUIToScreen(BindingRect, position);
        }

        public void OnPointerExit(PointerEventData eventData) => Hide();

    }
}