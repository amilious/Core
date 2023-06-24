using System;
using System.Collections.Generic;

namespace Amilious.Core.Definitions {
    
    [Serializable]
    public class DefineInfo {

        public string name;
        public string color = "";
        public string assemblyDefinitionPath;
        public List<string> defines = new List<string>();

        public DefineInfo(string name, string assemblyDefinitionPath) {
            this.name = name;
            this.assemblyDefinitionPath = assemblyDefinitionPath;
        }
        
    }

}