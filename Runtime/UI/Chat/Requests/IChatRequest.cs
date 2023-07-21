using System;
using TMPro;

namespace Amilious.Core.UI.Chat.Requests {

    public interface IChatRequest {

        /// <summary>
        /// This event will be triggered when the request is complete.
        /// </summary>
        public event Action<IChatRequest> OnComplete;

        /// <summary>
        /// The number of steps for the request.
        /// </summary>
        public int Steps { get; }

        /// <summary>
        /// True if the request was canceled.
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// This method will be used to get the prompt for the request.
        /// </summary>
        /// <param name="manager">The request manager.</param>
        /// <param name="step">The current step.</param>
        /// <param name="contentType">The input content type.</param>
        /// <returns>The string that should be displayed.</returns>
        public string GetPrompt(ChatRequestManager manager, int step, out TMP_InputField.ContentType contentType);

        /// <summary>
        /// This method will be used to validate the given input. 
        /// </summary>
        /// <param name="manager">The request manager.</param>
        /// <param name="step">The current step.</param>
        /// <param name="input">The input that was submitted.</param>
        /// <returns>The step that should be ran next.</returns>
        public int ValidateInput(ChatRequestManager manager, int step, string input);

        /// <summary>
        /// This method is called when the request has completed.
        /// </summary>
        public void RequestComplete();

    }
}