using IntMed.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Queries
{
    public class GetAllAgendasResponse
    {
        public int AgendaId { get; set; }
        public Medico Medico { get; set; }
        public DateTime Dia { get; set; }
        public List<string> Horarios { get; set; }
    }
}
