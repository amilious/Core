using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Amilious.Core.Chat {
    public class MessageData {
        
        private float _minY;
        private float _maxY;
        private float _yPos;
        private float _height;
        private DateTime _timeStamp;
        
        public string Text { get; }

        public string PlainText => StripRichTextTags(Text);
        
        public uint? Sender { get; }

        public uint? Group { get; }

        public DateTime TimeStamp => _timeStamp.ToLocalTime();

        public float YPos {
            get => _yPos;
            set {
                if(_yPos == value) return;
                _yPos = value;
                CalculateMinMax();
            }
        }

        public float Height { get => _height;
            set {
                if(_height == value) return;
                _height = value;
                CalculateMinMax();
            } 
        }
        
        public TMP_Text TextObject { get; set; }

        public MessageData(string message, uint? sender = null, uint? group = null, 
            float yPos=0, float height = 0, DateTime? timeStamp = null) {
            Text = message;
            Sender = sender;
            Group = group;
            _timeStamp = timeStamp ?? DateTime.UtcNow;
            _yPos = yPos;
            _height = height;
            CalculateMinMax();
        }

        public bool ShouldDraw(float minY, float maxY) => _minY <= maxY && _maxY >= minY;
        
        private void CalculateMinMax() {
            _maxY = _yPos;
            _minY = _yPos - _height;
        }
        
        private string StripRichTextTags(string input) {
            // Remove opening tags
            var output = Regex.Replace(input, @"<[^>]+?>", "");
            // Remove closing tags
            output = Regex.Replace(output, @"<\/[^>]+?>", "");
            return output;
        }

        public void DisplayText(TMP_Text text) {
            var rectTrans = (RectTransform)text.transform;
            var pos = rectTrans.localPosition;
            var size = rectTrans.sizeDelta;
            pos.y = YPos;
            size.y = _height;
            rectTrans.localPosition = pos;
            rectTrans.sizeDelta = size;
            text.SetText(Text);
            TextObject = text;
        }

    }
}