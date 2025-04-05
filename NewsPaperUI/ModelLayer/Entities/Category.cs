using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public Category()
        {
            Agendas = new HashSet<Agenda>();
            Announcements = new HashSet<Announcement>();
            Economies = new HashSet<Economy>();
            Homes = new HashSet<Home>();
            Specials = new HashSet<Special>();
            Sports = new HashSet<Sport>();
        }


        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public string PhotoPath { get; set; }

        public ICollection<Agenda> Agendas { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<Economy> Economies { get; set; }
        public ICollection<Home> Homes { get; set; }
        public ICollection<Special> Specials { get; set; }
        public ICollection<Sport> Sports { get; set; }
    }
}
