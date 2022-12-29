using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Amilious.Core.TextUtils {
    public class LinkActionItem {

        private readonly GameObject _gameObject;
        private readonly TMP_Text _name;
        private Action _clickAction;
        private LinkActionMenu _linkMenu;

        public LinkActionItem(GameObject prefab, LinkActionMenu parent) {
            _linkMenu = parent;
            _gameObject = Object.Instantiate(prefab, parent.transform);
            var listener = _gameObject.AddComponent<UIEnterExitListener>();
            listener.OnMouseEnterUI += OnMouseEnter;
            listener.OnMouseExitUI += OnMouseExit;
            var button = _gameObject.GetComponent<Button>();
            button.onClick.AddListener(OnActionClicked);
            _name = button.GetComponentInChildren<TMP_Text>();
        }

        private void OnMouseEnter() => _name.color = _linkMenu.MouseOverColor;

        private void OnMouseExit() => _name.color = _linkMenu.MouseNotOverColor;

        public void Hide() {
            _gameObject.SetActive(false);
            _gameObject.name = "pooled";
            _clickAction = null;
        }

        public void SetUpAction(string name, Action clickAction) {
            _name.text = name;
            _clickAction = clickAction;
            _gameObject.transform.SetAsLastSibling();
            _gameObject.name = name;
            _gameObject.SetActive(true);
        }
        
        private void OnActionClicked() {
            _clickAction?.Invoke();
            _linkMenu.Hide();
        }
        
    }
}