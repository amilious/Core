using System;
using System.Linq;
using System.Collections.Generic;
using Amilious.Core.Extensions;

namespace Amilious.Core.UI.PopupMenu {
    
    public class PopupMenu {

        private Dictionary<string, PopupMenuItem> MenuItems = new Dictionary<string, PopupMenuItem>();
        
        public bool AddMenuItem(string name, Action onClick, Func<bool> disable = null, Func<bool> hide = null) {
            var nameParts = name.Split('/');
            if(nameParts.Length == 0) return false;
            if(MenuItems.TryGetValueFix(nameParts[0], out var item)) {
                return item.AddMenuItem(nameParts, 1, onClick, disable, hide);
            }
            item = new PopupMenuItem(nameParts,1,onClick,disable,hide);
            MenuItems.Add(nameParts[0],item);
            return true;
        }
        
    }
    
    internal class PopupMenuItem {

        private Dictionary<string, PopupMenuItem> MenuItems = new Dictionary<string, PopupMenuItem>();

        public string Name { get; private set; }
        
        public Action OnClick { get; private set; }
        
        public Func<bool> CheckIfDisabled { get; private set; }
        
        public Func<bool> CheckIfHidden { get; private set; }

        public PopupMenuItem(string[] nameParts, int startIndex, Action onClick, Func<bool> disable = null, 
            Func<bool> hide = null) {
            Name = string.Join("/", nameParts.Take(startIndex + 1));
            if(nameParts.Length == startIndex + 1) {
                OnClick = onClick;
                CheckIfDisabled = disable;
                CheckIfHidden = hide;
            }else AddMenuItem(nameParts, startIndex, onClick, disable, hide);
        }

        public bool AddMenuItem(string[] nameParts, int startIndex, Action onClick, Func<bool> disable, Func<bool> hide) {
            if(nameParts.Length-1 <= startIndex) return false;
            if(MenuItems.TryGetValueFix(nameParts[startIndex], out var item)) {
                return item.AddMenuItem(nameParts, startIndex + 1, onClick, disable, hide);
            }
            item = new PopupMenuItem(nameParts, startIndex + 1, onClick, disable, hide);
            MenuItems.Add(nameParts[startIndex],item);
            return true;
        }
        
    }
    
}