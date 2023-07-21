using System;
using System.Reflection;

namespace Amilious.Core.Attributes {
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AmiButtonAttribute : Attribute {

        public string Name { get; }
        
        public bool OnlyWhenPlaying { get; }
        
        public MethodInfo MethodInfo { get; set; }
        
        public AmiModifierAttribute[] Modifiers { get; set; }
        
        public AmiButtonAttribute(string name, bool onlyWhenPlaying = true) {
            Name = name;
            OnlyWhenPlaying = onlyWhenPlaying;
        }
        
    }
}