
using TMPro;
using System;
using UnityEngine;
using Cysharp.Text;
using UnityEngine.UI;
using Amilious.Console;
using Amilious.Core.Input;
using Amilious.Core.UI.Text;
using UnityEngine.EventSystems;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.Core.Identity.User;
using Amilious.Core.Identity.Group;
using System.Text.RegularExpressions;
using Amilious.Core.UI.Component;

namespace Amilious.Core.UI.Chat {
    
    [AddComponentMenu("Amilious/UI/Chat/Chat Box")]
    [HelpURL("https://amilious.gitbook.io/core/runtime/ui/chatbox")]
    [DisallowMultipleComponent, RequireComponent(typeof(UIComponent))]
    public class ChatBox : AmiliousBehavior, IChatBox {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string COMPONENTS = "Components";
        private const string DRAWING = "Drawing";
        private const string COMMANDS = "Commands";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, AmiTab(COMPONENTS)] private ScrollRect scrollRect;
        [SerializeField, AmiTab(COMPONENTS)] private TMP_InputField inputField;
        [SerializeField, AmiTab(COMPONENTS)] private GameObject messagePrefab;
        [SerializeField, AmiTab(COMPONENTS)] private TMP_Text chatName;
        [SerializeField, AmiTab(COMPONENTS)] private TMP_Text promptText;
        [SerializeField, AmiTab(COMPONENTS)] private GameObject promptArea;

        [Tooltip("This value will adjust the drawn content size to extend in both directions from the visible area.")]
        [SerializeField, Min(0), AmiTab(DRAWING)] private float extraRender = 10f;
        [Tooltip("This value is used to add extra space on top or bottom of the messages.")]
        [AmiVector(VLayout.SingleLine,"top","bottom"), AmiTab(DRAWING), SerializeField] 
        private Vector2 padding = new (5,5);

        [SerializeField, AmiTab(COMMANDS)] private bool usingCommands = false;
        [SerializeField, AmiTab(COMMANDS), AmiShowIf(nameof(usingCommands))] private string commandPrefix = "/";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private float _minY;
        private float _maxY;
        private float _offset;
        private bool _handlingRequest;
        private TMP_Text _template;
        private int _lastIndex = -1;
        private int _firstIndex = -1;
        private float _scrollBarWidth;
        private ChatLinkManager _linkManager;
        private RectTransform _rectTransform;
        private RectTransform _scrollRectTransform;
        private string _lastInputText = string.Empty;

        private UIComponent _component;
        
        private readonly HashSet<int> _drawn = new HashSet<int>();
        private readonly List<MessageData> _messages = new List<MessageData>();
        private readonly Queue<TMP_Text> _textObjectsQueue = new Queue<TMP_Text>();
        private static readonly Regex LinkPattern = 
            new Regex(@"(?<!<link=web>)(http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?)");

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public UIComponent UIComponent => this.GetCacheComponent(ref _component);
        
        public ChatType ChatType { get; private set; }
        
        public uint ChatId { get; private set; }

        public string InputText => inputField.text;

        public ChatLinkManager LinkManager => this.GetCacheComponent(ref _linkManager);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        public event IChatBox.MessageDelegate OnMessageSubmitted;
        /// <inheritdoc />
        public event Action<string> OnCommandSubmitted;
        /// <inheritdoc />
        public event IChatBox.InputUpdatedDelegate OnInputTextUpdated;
        /// <inheritdoc />
        public event Action<string> OnInputRequestResult;

        #region Monobehavior Methods ///////////////////////////////////////////////////////////////////////////////////
        
        private void Awake() {
            _rectTransform = ((RectTransform)gameObject.transform);
            _scrollRectTransform = (RectTransform)scrollRect.transform;
            _scrollBarWidth = ((RectTransform)scrollRect.verticalScrollbar.transform).sizeDelta.x;
            var text = Instantiate(messagePrefab, scrollRect.content);
            text.name = "Calculation Template";
            _template = text.GetComponent<TMP_Text>();
            _template.SetText("");
            scrollRect.normalizedPosition = new Vector2(0, 0);
            SetContentHeight(0);
            OnScrollValueChanged(scrollRect.normalizedPosition);
            Redraw();
        }

        private void OnEnable() {
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            inputField.onSubmit.AddListener(OnInputSubmit);
            inputField.onValueChanged.AddListener(OnInputChanged);
            UIComponent.AddOnExitStateListener(OnExitState);
            UIComponent.AddOnEnterStateListener(OnEnterState);
        }

        private void OnDisable() {
            scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
            inputField.onSubmit.RemoveListener(OnInputSubmit);
            inputField.onValueChanged.RemoveListener(OnInputChanged);
            UIComponent.RemoveOnExitStateListener(OnExitState);
            UIComponent.RemoveOnEnterStateListener(OnEnterState);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a message to the chat box.
        /// </summary> 
        /// <param name="message">The message text.</param>
        /// <param name="sender">(optional)The sender of the message.</param>
        /// <param name="group">(optional)The group that the message is for. </param>
        /// ReSharper disable once MemberCanBePrivate.Global
        public void AddMessage(string message, uint? sender = null, uint? group=null) {
            //fix weblinks
            message = LinkPattern.Replace(message,"<link=web>$&</link>");
            var height = GetTextHeight(message);
            var messageData = new MessageData(message, sender, group, _offset-padding.x, height);
            _messages.Add(messageData);
            var atBottom = scrollRect.verticalNormalizedPosition <= 0.00001||
                Mathf.Abs(_offset)<_scrollRectTransform.rect.height;
            _offset -= height;
            SetContentHeight(_offset);
            if(!atBottom) Redraw();
            else scrollRect.normalizedPosition = new Vector2(0, 0);
        }

        /// <summary>
        /// This method is used to draw the messages.
        /// </summary>
        /// <param name="force">If true redundancy checks will be skipped.</param>
        /// ReSharper disable once MemberCanBePrivate.Global
        public void Redraw(bool force = false) {
            //skip redraw if loaded messages have not changed
            if (!force && !ShouldDraw(_firstIndex-1) && ShouldDraw(_firstIndex) && 
                ShouldDraw(_lastIndex) && !ShouldDraw(_lastIndex + 1)) return;
            // Calculate the visible range of indices
            _firstIndex = _messages.FindIndex(msg => msg.ShouldDraw(_minY, _maxY));
            _lastIndex = _messages.FindLastIndex(msg => msg.ShouldDraw(_minY, _maxY));
            if (_firstIndex == -1 || _lastIndex == -1) {
                // No visible messages, clear all drawn messages
                foreach (var id in _drawn) ReturnTextObject(_messages[id]);
                _drawn.Clear();
                return;
            }

            // Remove messages that are no longer visible
            var removed = new HashSet<int>(_drawn);
            foreach (var id in removed) {
                if(id >= _firstIndex && id <= _lastIndex) continue;
                ReturnTextObject(_messages[id]);
                _drawn.Remove(id);
            }

            // Draw visible messages
            for (var i = _firstIndex; i <= _lastIndex; i++) {
                if(_drawn.Contains(i)) continue;
                var msg = _messages[i];
                var textObject = GetTextObject();
                msg.DisplayText(textObject);
                _drawn.Add(i);
            }
        }

        /// <summary>
        /// This method is used to recalculate the sizes and redraw the messages.
        /// </summary>
        public void RecalculateSize() {
            Canvas.ForceUpdateCanvases(); //make sure all values are up to date
            _offset = 0;
            foreach(var msg in _messages) {
                msg.YPos = _offset-padding.x;
                msg.Height = GetTextHeight(msg.Text);
                if(msg.TextObject!=null)msg.DisplayText(msg.TextObject);
                _offset -= msg.Height;
            }
            SetContentHeight(_offset);
            Redraw(true);
        }

        /// <inheritdoc />
        public void SetCommandPrefix(string prefix) {
            usingCommands = true;
            commandPrefix = prefix;
        }

        /// <inheritdoc />
        public void SetInputText(string text, bool silent = false) {
            if(inputField == null) return;
            if(!silent) inputField.text = text;
            else inputField.SetTextWithoutNotify(text);
            Canvas.ForceUpdateCanvases();
            FocusInput();
        }

        /// <inheritdoc />
        public void FocusInput(bool moveCaret = true) {
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            inputField.Select();
            inputField.ActivateInputField();
            if(!moveCaret) return;
            inputField.caretPosition = inputField.text.Length;
            inputField.MoveToEndOfLine(false, true);
        }

        public void RemoveInputFocus() {
            inputField.DeactivateInputField();
        }

        /// <inheritdoc />
        public void SubmitCurrentInput() {
            ExecuteEvents.Execute(inputField.gameObject, null, ExecuteEvents.submitHandler);
        }

        /// <inheritdoc />
        public bool SetCommandText(string command) {
            if(!usingCommands) return false;
            SetInputText($"{commandPrefix}{command}");
            return true;
        }

        /// <inheritdoc />
        public void ClearChatChannel() {
            ChatType = ChatType.Global;
            ChatId = 0;
            chatName.SetText("");
            chatName.gameObject.SetActive(false);
        }

        /// <inheritdoc />
        public void SetChatChannel(UserIdentity user) {
            ChatType = ChatType.Private;
            ChatId = user;
            chatName.SetText(ZString.StyleFormat(StyleFormat.PrivateSentText,"{0} ->",user.Name));
            chatName.gameObject.SetActive(true);
            SetInputText(string.Empty);
        }

        /// <inheritdoc />
        public void SetChatChannel(GroupIdentity group) {
            ChatType = ChatType.Group;
            ChatId = group;
            chatName.SetText(ZString.Style(StyleFormat.Group,group.Name));
            chatName.gameObject.SetActive(true);
            SetInputText(string.Empty);
        }

        /// <inheritdoc />
        public string GetMessageText() {
            var sb = StringBuilderPool.Rent;
            for(var i = 0; i < _messages.Count; i++) {
                if(i != 0) sb.NewLine();
                sb.Append(_messages[i].PlainText);
            }
            return sb.ToStringAndReturnToPool();
        }

        /// <inheritdoc />
        public string GetMessageText(int numberOfMessages) {
            var sb = StringBuilderPool.Rent;
            var startIndex = _messages.Count - 1 - numberOfMessages;
            for(var i = startIndex; i < _messages.Count; i++) {
                if(i != startIndex) sb.NewLine();
                sb.Append(_messages[i].PlainText);
            }
            return sb.ToStringAndReturnToPool();
        }

        public void ClearMessages() {
            foreach(var index in _drawn) ReturnTextObject(_messages[index]);
            _drawn.Clear();
            _messages.Clear();
            RecalculateSize();
        }
        
        public void MakeInputRequest(string message, TMP_InputField.ContentType contentType) {
            _handlingRequest = true;
            inputField.contentType = contentType;
            promptArea.SetActive(true);
            scrollRect.gameObject.SetActive(false);
            promptText.SetText(message);
            ClearChatChannel();
            SetInputText(string.Empty);
        }
        
        public void CancelInputRequest() {
            inputField.contentType = TMP_InputField.ContentType.Standard;
            promptText.SetText(string.Empty);
            promptArea.SetActive(false);
            scrollRect.gameObject.SetActive(true);
            _handlingRequest = false;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        private void OnInputChanged(string text) {
            if(text.Length == 0 && InputHelper.GetKey(KeyCode.Backspace)&&_lastInputText.Length == 0) 
                ClearChatChannel();


            OnInputTextUpdated?.Invoke(text,_handlingRequest);
            _lastInputText = text;
        }
        
        private void SetContentHeight(float height) {
            height = Mathf.Abs(height) + padding.x + padding.y;
            //var startHeight = height;
            //height = Mathf.Max(height, _scrollRectTransform.sizeDelta.y);
            scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            //if(height == startHeight) return;
            OnScrollValueChanged(scrollRect.normalizedPosition);
        }
        
        /// <summary>
        /// This method is called when the input text is submitted.
        /// </summary>
        /// <param name="inputText">The input text.</param>
        private void OnInputSubmit(string inputText) {
            inputField.text = string.Empty;
            inputField.ActivateInputField();
            if(string.IsNullOrWhiteSpace(inputText)) return;
            if(_handlingRequest) {
                CancelInputRequest();
                OnInputRequestResult?.Invoke(inputText);
                return;
            }
            if(usingCommands&&inputText.StartsWith(commandPrefix))
                OnCommandSubmitted?.Invoke(inputText);
            else OnMessageSubmitted?.Invoke(ChatType, inputText, ChatId);
        }
        
        /// <summary>
        /// This method is called when the scroll bar location changes.
        /// </summary>
        /// <param name="scroll">The scroll position in percentage.</param>
        private void OnScrollValueChanged(Vector2 scroll) {
            var height = _scrollRectTransform.sizeDelta.y;
            _maxY = (_offset + height-padding.x-padding.y) * (1-scroll.y);
            _minY = _maxY - height;
            _minY -= extraRender;
            _maxY += extraRender;
            Redraw();
        }
        
        /// <summary>
        /// This method is used to get or create a free text object.
        /// </summary>
        /// <returns>The text object.</returns>
        private TMP_Text GetTextObject() {
            if(_textObjectsQueue.TryDequeue(out var chatObject)) {
                chatObject.gameObject.SetActive(true);
                return chatObject;
            }
            var text = Instantiate(messagePrefab, scrollRect.content);
            chatObject = text.GetComponent<TMP_Text>();
            var linkListener = text.GetComponent<TMProLinkHandler>();
            if(linkListener==null)Debug.Log("Something went wrong.");
            if(LinkManager!=null) LinkManager.Register(linkListener);
            return chatObject;
        }

        private void ReturnTextObject(MessageData message) {
            var textObject = message.TextObject;
            message.TextObject = null;
            if(textObject==null)return;
            textObject.text = string.Empty;
            textObject.gameObject.SetActive(false);
            _textObjectsQueue.Enqueue(textObject);
        }
        
        /// <summary>
        /// This method is used to get the height for the given text.
        /// </summary>
        /// <param name="text">The text that you want to get the height for.</param>
        /// <returns>The height of the given text.</returns>
        private float GetTextHeight(string text) {
            var width = _scrollRectTransform.sizeDelta.x - _scrollBarWidth;
            return _template.GetPreferredValues(text,width,0).y;
        }

        /// <summary>
        /// This method is used to check if the message with the given id should be drawn.
        /// </summary>
        /// <param name="id">The id of the message that you want to check.</param>
        /// <returns>True if the message should be drawn, otherwise false.</returns>
        private bool ShouldDraw(int id) {
            if(id < 0 || id >= _messages.Count) return false;
            return _messages[id].ShouldDraw(_minY, _maxY);
        }
        
        private void OnExitState(UIStatesType state) {
            if(state != UIStatesType.Resizing) return;
            scrollRect.gameObject.SetActive(true);
            inputField.transform.parent.gameObject.SetActive(true);
            RecalculateSize();
        }

        private void OnEnterState(UIStatesType state) {
            if(state != UIStatesType.Resizing) return;
            scrollRect.gameObject.SetActive(false);
            inputField.transform.parent.gameObject.SetActive(false);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}