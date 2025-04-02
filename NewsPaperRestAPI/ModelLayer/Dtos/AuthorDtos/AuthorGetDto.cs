using InfrastructorLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dtos.AuthorDtos
{
   public  class AuthorGetDto:InterfaceDataTransferObject
    {
        public string FirstName { get; set; }  // Ad 
        public string LastName { get; set; }   // Soyad
        public string Biography { get; set; }  // Biyografi
        public string CoverImage { get; set; } // Kapak Resmi
        public string Phone { get; set; } // Telefor 
        public string Email { get; set; } // Email  

    }
}
