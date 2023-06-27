using IntMed.Application.Commands.Consultas.Requests;
using IntMed.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Consulta.CommandHandlers
{
    internal class DeleteConsultaHandler : IRequestHandler<DeleteConsultaRequest>
    {
        private readonly IConsultaRepository _consultaRepository;

        public DeleteConsultaHandler(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }
        public async Task Handle(DeleteConsultaRequest request, CancellationToken cancellationToken)
        {
             await _consultaRepository.DeleteConsulta(request.ConsultaId); 
        }
    }
}
