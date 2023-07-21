using System;
using TMPro;

namespace Amilious.Core.UI.Chat.Requests {
    
    public abstract class AbstractChatRequest : IChatRequest {

        public bool TryStartRequest() => ChatRequestManager.TryMakeRequest(this);

        public event Action<IChatRequest> OnComplete;
        public abstract int Steps { get; }
        public bool Canceled { get; set; }
        public abstract string GetPrompt(ChatRequestManager manager, int step,
            out TMP_InputField.ContentType contentType);

        public abstract int ValidateInput(ChatRequestManager manager, int step, string input);

        public void RequestComplete() => OnComplete?.Invoke(this);
    }
    
}