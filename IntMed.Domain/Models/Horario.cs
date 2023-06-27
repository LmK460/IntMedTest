using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Domain.Models
{
    public class Horario
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string AgendaId { get; set; }
    }
}
