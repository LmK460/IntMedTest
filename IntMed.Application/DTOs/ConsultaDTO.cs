using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.DTOs
{
    public class ConsultaDTO
    {
        public int Id { get; set; }
        public int AgendaId { get; set; }
        public DateTime DataAgendamento { get; set; }

        public int MedicoId { get; set; }

        public DateTime Horario { get; set; }
    }
}
