using IntMed.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Queries
{
    public class GetAllConsultasResponse : IRequest
    {
        public int Id { get; set; }

        public DateTime DataAgendamento { get; set; }

        public DateTime Horario { get; set; }

        public Medico Medico { get; set; }
    }
}
