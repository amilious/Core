using System;

namespace Amilious.Core.Attributes {
    
    [AttributeUsage(AttributeTargets.Field)]
    public class AmiliousDisableAttribute : AmiliousModifierAttribute {
        public override bool ShouldHide<T>(T property) => false;

        public override bool ShouldDisable<T>(T property) => true;
    }
}