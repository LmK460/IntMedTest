using IntMed.Application.Commands.Consultas.Response;
using IntMed.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Consultas.Requests
{
    public class CreateConsultaRequest : IRequest<CreateConsultaResponse>
    {
        public string AgendaId { get; set; }
        public string Horario { get; set; }
    }
}
