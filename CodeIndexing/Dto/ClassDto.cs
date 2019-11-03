using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeIndexing.Dto
{
    public class ClassDto
    {
        [Key]
        public int ClassId { get; set; }
        public TypeCode UnderlyingType { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
