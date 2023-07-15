using System;
using System.Collections.Generic;
using Amilious.Core.Chat.Requests;
using Amilious.Core.Extensions;
using TMPro;
using UnityEngine;

namespace Amilious.Core.UI.Chat {
    
    [RequireComponent(typeof(ChatBox))]
    public class ChatRequestManager : AmiliousBehavior {

        #region Static Stuff ///////////////////////////////////////////////////////////////////////////////////////////
        
        private static List<ChatRequestManager> Instances = new List<ChatRequestManager>();
        private static Queue<IChatRequest> Requests = new Queue<IChatRequest>();
        private static IChatRequest CurrentRequest;
        private static int Step = 0;
        
        public static bool TryMakeRequest(IChatRequest chatRequest) {
            if(Instances.Count == 0) return false;
            if(CurrentRequest!=null) {
                Requests.Enqueue(chatRequest);
                return true;
            }
            StartRequest(chatRequest);
            return true;
        }

        private static void StartRequest(IChatRequest request) {
            if(!TryGetFirstInstance(out var firstManager)||request.Steps<1) {
                EndRequest(true); return;
            }
            CurrentRequest = request;
            Step = 0;
            var prompt = CurrentRequest.GetPrompt(firstManager, Step, out var contentType);
            foreach(var manager in Instances) manager.SetPrompt(prompt, contentType);
        }

        private static bool TryGetFirstInstance(out ChatRequestManager manager) {
            manager = null;
            if(Instances.Count < 1) return false;
            manager = Instances[0];
            return true;
        }

        private static void EndRequest(bool cancel = false) {
            if(CurrentRequest == null) return;
            if(cancel)CurrentRequest.Canceled = true;
            CurrentRequest.RequestComplete();
            CurrentRequest = null;
            foreach(var manager in Instances) manager.CancelRequest();
            if(Requests.TryDequeue(out var next)) StartRequest(next);
        }

        private static void GetRequestInput(string input) {
            if(CurrentRequest == null) return;
            if(!TryGetFirstInstance(out var firstManager)||CurrentRequest.Canceled) {
                EndRequest(true); return;
            }

            var nextStep = CurrentRequest.ValidateInput(firstManager, Step, input);

            //the validation failed so try again
            if(nextStep == Step) { 
                var prompt2 = CurrentRequest.GetPrompt(firstManager, Step, out var contentType);
                foreach(var manager in Instances) manager.SetPrompt(prompt2,contentType);
                return;
            }

            //invalid step returned so cancel
            if(nextStep < 0||nextStep>CurrentRequest.Steps||CurrentRequest.Canceled) {
                EndRequest(true);
                return;
            }

            //the request is complete.
            if(nextStep == CurrentRequest.Steps) {
                EndRequest();
                return;
            }

            //move on to nest step
            Step = nextStep;
            var prompt = CurrentRequest.GetPrompt(firstManager, Step, out var contentType2);
            foreach(var manager in Instances) manager.SetPrompt(prompt,contentType2);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private ChatBox _chatBox;

        public ChatBox ChatBox => this.GetCacheComponent(ref _chatBox);

        private void Awake() {
            if(!Instances.Contains(this)) Instances.Add(this);
        }

        private void OnEnable() {
            ChatBox.OnInputRequestResult += OnInputRequestResult;
            if(!Instances.Contains(this)) Instances.Add(this);
        }

        private void OnInputRequestResult(string text) => GetRequestInput(text);

        private void OnDisable() { Instances.Remove(this); }

        private void SetPrompt(string message, TMP_InputField.ContentType contentType) {
            ChatBox.MakeInputRequest(message, contentType);
        }

        private void CancelRequest() => ChatBox.CancelInputRequest();

    }

}