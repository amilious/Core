using System;
using Amilious.Core.Extensions;
using TMPro;

namespace Amilious.Core.UI.Chat.Requests {
    
    public class NewPasswordChatRequest : AbstractChatRequest {

        private string _step0Fail;
        public override int Steps => 2;
        public int MinLength { get; }
        public int MaxLength { get; }
        public bool RequireUpper { get; }
        public bool RequireLower { get; }
        public bool RequireNumber { get; }
        public bool RequireSpecial { get; }
        
        public string Password { get; private set; }
        
        /// <summary>
        /// This method is used to create a new password validator instance.
        /// </summary>
        /// <param name="minLength">The minimum length of the password.</param>
        /// <param name="maxLength">The maximum length of the password.</param>
        /// <param name="requireUpper">If true the password requires an uppercase letter.</param>
        /// <param name="requireLower">If true the password requires a lowercase letter.</param>
        /// <param name="requireNumber">If true the password requires a number.</param>
        /// <param name="requireSpecial">If true the password requires a special character.</param>
        public NewPasswordChatRequest(int minLength = 6, int maxLength = 20, bool requireUpper = true, 
            bool requireLower = true, bool requireNumber = true, bool requireSpecial = true) {
            MinLength = minLength;
            MaxLength = maxLength;
            RequireUpper = requireUpper;
            RequireLower = requireLower;
            RequireNumber = requireNumber;
            RequireSpecial = requireSpecial;
        }

        public override string GetPrompt(ChatRequestManager manager, int step, out TMP_InputField.ContentType contentType) {
            contentType = TMP_InputField.ContentType.Password;
            return step == 0 ? Step0Prompt() : Step1Prompt();
        }

        public override int ValidateInput(ChatRequestManager manager, int step, string input) {
            return step == 0 ? ValidateStep0(input) : ValidateStep1(input);
        }

        private string Step0Prompt() {
            if(_step0Fail==null) return "Please enter a new password!";
            return $"{_step0Fail}\n\nPlease enter a new password!";
        }

        private string Step1Prompt() => "Please confirm your password!";

        private int ValidateStep0(string input) {
            if(!IsValid(input, out var fail)) {
                _step0Fail = fail;
                return 0;
            }
            Password = input;
            return 1;
        }
        
        private int ValidateStep1(string input) {
            if(input.Equals(Password, StringComparison.InvariantCulture)) return 2;
            _step0Fail = "The provided passwords did not match!";
            return 0;
        }
        
        private bool IsValid(string value, out string response) {
            var hasUpper = !RequireUpper;
            var hasLower = !RequireLower;
            var hasNumber = !RequireNumber;
            var hasSpecial = !RequireSpecial;
            if(value.Length < MinLength) {
                response = $"The password is too short!  It must contain at least {MinLength} characters!";
                return false;
            }
            if(value.Length > MaxLength) {
                response = $"The password is too long!  The password cannot contain more than {MaxLength} characters";
            }
            //check every character
            foreach(var c in value) {
                if(char.IsLetter(c)&&char.IsUpper(c)) hasUpper = true;
                if(char.IsLetter(c)&&char.IsLower(c)) hasLower = true;
                if(char.IsNumber(c)) hasNumber = true;
                if(c.IsSpecialCharacter()) hasSpecial = true;
            }
            if(!hasUpper) {
                response = "The password requires an upper case letter!";
                return false;
            }
            if(!hasLower) {
                response = "The password requires a lower case letter!";
                return false;
            }
            if(!hasNumber) {
                response = "The password requires a number!";
                return false;
            }
            if(!hasSpecial) {
                response = "The password requires a special character!";
                return false;
            }
            response = string.Empty;
            return true;
        }
        
    }
}