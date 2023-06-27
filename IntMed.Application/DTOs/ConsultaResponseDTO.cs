using IntMed.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.DTOs
{
    internal class ConsultaResponseDTO
    {
        public string ConsultaId { get; set; }
        public string Dia { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime horario { get; set; }
        public Medico Medico { get; set; }
    }
}
