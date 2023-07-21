using System;
using Amilious.Core.Identity.Group;
using Amilious.Core.Identity.User;

namespace Amilious.Core.UI.Chat {
    
    public interface IChatBox {

        public string InputText { get; }
        
        public delegate void MessageDelegate(ChatType type, string message, uint id);

        public delegate void InputUpdatedDelegate(string text, bool handlingRequest);

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when a message is submitted.
        /// </summary>
        public event MessageDelegate OnMessageSubmitted;

        /// <summary>
        /// This event is triggered when a command is submitted.
        /// </summary>
        public event Action<string> OnCommandSubmitted;

        /// <summary>
        /// This event is triggered when the input text is updated.
        /// </summary>
        public event InputUpdatedDelegate OnInputTextUpdated;
        
        /// <summary>
        /// This event is triggered when the result of an input request is submitted.
        /// </summary>
        public event Action<string> OnInputRequestResult;

        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to add a message to the chat box.
        /// </summary> 
        /// <param name="message">The message text.</param>
        /// <param name="sender">(optional)The sender of the message.</param>
        /// <param name="group">(optional)The group that the message is for. </param>
        /// ReSharper disable once MemberCanBePrivate.Global
        void AddMessage(string message, uint? sender = null, uint? group = null);
        
        /// <summary>
        /// This method is used to set the command prefix that will be used to differentiate between
        /// commands and messages.
        /// </summary>
        /// <param name="prefix">The command prefix.</param>
        void SetCommandPrefix(string prefix);

        /// <summary>
        /// This method is used to set the input text.
        /// </summary>
        /// <param name="text">The text that you want to set as the input text.</param>
        /// <param name="silent">If true, the text will be set silently, otherwise normally.</param>
        void SetInputText(string text, bool silent = false);

        /// <summary>
        /// This method is used to set the input to the given command.
        /// </summary>
        /// <param name="command">The command text without the command prefix.</param>
        /// <returns>True if commands are enabled, otherwise false.</returns>
        bool SetCommandText(string command);
        
        /// <summary>
        /// This method is used to reset the chat channel back to global.
        /// </summary>
        public void ClearChatChannel();

        /// <summary>
        /// This method is used to set the chat channel to whisper a user.
        /// </summary>
        /// <param name="user">The user that you want to whisper.</param>
        public void SetChatChannel(UserIdentity user);

        /// <summary>
        /// This method is used to set the chat channel to a group.
        /// </summary>
        /// <param name="group">The group that you want to set the channel to.</param>
        public void SetChatChannel(GroupIdentity group);

        /// <summary>
        /// This method is used to get the message text.
        /// </summary>
        /// <returns>The text of all of the messages.</returns>
        string GetMessageText();

        /// <summary>
        /// This method is used to get the message text for the last messages.
        /// </summary>
        /// <param name="numberOfMessages">The number of messages that you want to get the text for.</param>
        /// <returns>The messages as a string.</returns>
        string GetMessageText(int numberOfMessages);

        /// <summary>
        /// This method is used to clear the messages.
        /// </summary>
        void ClearMessages();

        void SubmitCurrentInput();

        void FocusInput(bool moveCaret = true);

        void RemoveInputFocus();
    }
}