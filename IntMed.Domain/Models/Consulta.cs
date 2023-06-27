using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Domain.Models
{
    public class Consulta
    {
        public string Id { get; set; }
        public Agenda Agenda { get; set; }
        public DateTime DataAgendamento { get; set; }

        public DateTime Horario { get; set; }
    }
}
