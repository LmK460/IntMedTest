using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Domain.Models
{
    public class Agenda
    {
        public string AgendaId { get; set; }
        public Medico Medico { get; set; }
        public DateTime Dia { get; set; }

        public List<Horario> Horarios { get; set; }
    }
}
