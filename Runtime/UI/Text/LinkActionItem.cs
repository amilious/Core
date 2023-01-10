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
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Amilious.Core.UI.Text {
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