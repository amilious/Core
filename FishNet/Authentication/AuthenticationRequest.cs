namespace Amilious.Core.FishNet.Authentication {
    
    public struct AuthenticationRequest {
        
        public int UserId { get; set; }
        public bool NewUser { get; set; }
        public int RequestId { get; set; }
        public string UserName { get; set; }
        public AuthenticationRequestType RequestType { get; set; }
        
    }
    
}