using InfrastructorLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dtos.CategoryDtos
{
   public  class CategoryGetDto:InterfaceDataTransferObject
    {
        public string Name { get; set; }    
        public string Description { get; set; }
        public string PhotoPath { get; set; }
    }
}
