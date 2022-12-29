using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Amilious.Core.TextUtils {
    
    public class LinkActionMenu : AmiliousBehavior, IPointerExitHandler {

        [SerializeField] private GameObject actionPrefab;
        [SerializeField] private Color mouseOverColor = Color.white;
        [SerializeField] private Color mouseNotOverColor = Color.gray;

        private readonly Queue<LinkActionItem> _actionItemPool = new Queue<LinkActionItem>();
        private readonly List<LinkActionItem> _currentActions = new List<LinkActionItem>();

        public Color MouseNotOverColor => mouseNotOverColor;

        public Color MouseOverColor => mouseOverColor;
        
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
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData) => Hide();
    }
}