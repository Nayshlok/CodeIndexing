using System;
using System.Collections.Generic;
using System.Text;

namespace CodeIndexing.Dto
{
    public class MethodRelationship
    {
        public int CallingMethodId { get; set; }
        public int MethodBeingCalledId { get; set; }

        public MethodDto CallingMethod { get; set; }
        public MethodDto MethodBeingCalled { get; set; }
    }
}
