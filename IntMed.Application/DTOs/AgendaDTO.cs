using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.DTOs
{
    public class AgendaDTO
    {
        public int AgendaId { get; set; }
        public int MedicoId { get; set; }
        public DateTime Dia { get; set; }

    }
}
