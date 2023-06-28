using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Consultas.Requests
{
    public class DeleteConsultaRequest :IRequest
    {
        public int ConsultaId { get; set; }
    }
}
