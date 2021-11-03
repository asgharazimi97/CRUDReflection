using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Entities.Models
{
    public class InstanceModel
    {
        public Type RepositoryType { get; set; }
        public object Repository { get; set; }
    }
}
