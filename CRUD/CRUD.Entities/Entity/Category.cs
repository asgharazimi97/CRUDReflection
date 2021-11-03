using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Entities.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<SubCategory> SubCategories { get; set; }
    }
}
