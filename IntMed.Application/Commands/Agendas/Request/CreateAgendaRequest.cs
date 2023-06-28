using IntMed.Application.Commands.Agendas.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Agendas.Request
{
    public class CreateAgendaRequest:IRequest<CreateAgendaResponse>
    {
        public int Med_id { get; set; }
        public DateTime Dia { get; set; }
        public List<string> horario { get; set; }
    }
}
