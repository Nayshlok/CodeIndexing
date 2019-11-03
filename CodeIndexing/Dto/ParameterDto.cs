using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeIndexing.Dto
{
    public class ParameterDto
    {
        [Key]
        public int ParameterId { get; set; }
        public ClassDto ParamterType { get; set; }
        public string ParameterName { get; set; }
        public MethodDto BelongsToMethod { get; set; }
    }
}
