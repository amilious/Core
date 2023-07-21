using Amilious.Core.UI.Chat.Requests;
using UnityEngine;

namespace Amilious.Core.Authentication {
    
    
    [AddComponentMenu("Amilious/Core/Networking/Chat Password Request")]
    public class ChatPasswordRequest : PasswordRequestProvider {

        private PasswordCreatedCallback _callback;
        private NewPasswordChatRequest _chatRequest;
        
        public override void RequestNewPassword(PasswordCreatedCallback passwordCreatedCallback) {
            _callback = passwordCreatedCallback;
            _chatRequest = new NewPasswordChatRequest();
            _chatRequest.OnComplete += OnPasswordComplete;
            _chatRequest.TryStartRequest();
        }

        private void OnPasswordComplete(IChatRequest obj) {
            if(_chatRequest.Canceled) {
                Debug.LogError("The new password request should not be canceled.");
                return;
            }
            _callback?.Invoke(_chatRequest.Password);
        }
    }
}