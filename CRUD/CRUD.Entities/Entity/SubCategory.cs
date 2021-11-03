using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Entities.Entity
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
