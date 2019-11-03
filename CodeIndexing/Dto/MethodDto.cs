using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeIndexing.Dto
{
    public class MethodDto
    {
        [Key]
        public int MethodId { get; set; }
        public string MethodName { get; set; }
        public string Namespace { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public ClassDto BelongsTo { get; set; }
        public IEnumerable<MethodRelationship> MethodsCalled { get; set; }
        public IEnumerable<MethodRelationship> CalledByMethods { get; set; }
        public IEnumerable<ParameterDto> Parameters { get; set; }
    }
}
