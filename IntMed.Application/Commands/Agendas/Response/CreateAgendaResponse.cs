using IntMed.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Agendas.Response
{
    public class CreateAgendaResponse : IRequest
    {
        public int AgendaId { get; set; }
        public Medico Medico { get; set; }
        public DateTime Dia { get; set; }
    }
}
