using System;
using System.Collections.Generic;

namespace LambdaFunction.Inputs
{
    public class InputFile
    {
        public string Id { get; set; }
        
        public DateTime ModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public string UniqueName { get; set; }
        
        public int Enumerator { get; set; }
        
        public string OriginalName { get; set; }
        
        public string Directory { get; set; }
        
        public string ContentType { get; set; }
        
        public long Size { get; set; }
        
        public Dictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();
        
        public bool CanDelete { get; set; }
        
        public bool CanRename { get; set; }
        
        public bool CanMove { get; set; }
        
        public List<InputFileOptimization> Optimizations { get; set; }
        
        public bool IsPublic { get; set; }
    }
}
