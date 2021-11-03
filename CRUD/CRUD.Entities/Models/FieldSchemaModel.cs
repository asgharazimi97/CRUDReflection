using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Entities.Models
{
    public class FieldSchemaModel
    {
        public string EnglishName { get; set; }
        public string DataType { get; set; }
        public bool IsForeignKey { get; set; } = false;
        public string ForeignKeyTable { get; set; }
        public bool IsEnum { get; set; } = false;
        public List<EnumModel> EnumInfo { get; set; }
    }

    public class EnumModel
    {
        public object Value { get; set; }
        public string Name { get; set; }
    }
}
