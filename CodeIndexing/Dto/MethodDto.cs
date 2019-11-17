using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        public string FilePathAndName
        {
            get
            {
                return FilePath + FileName;
            }
            set
            {
                FileName = Path.GetFileName(value);
                FilePath = value.Substring(0, value.Length - FileName.Length);
            }
        }
        public string ReturnType { get; set; }
        public ClassDto BelongsToClass { get; set; }
        public IEnumerable<MethodRelationship> MethodsCalled { get; set; }
        public IEnumerable<MethodRelationship> CalledByMethods { get; set; }
        public List<ParameterDto> Parameters { get; set; } = new List<ParameterDto>();
    }
}
