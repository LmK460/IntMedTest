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
            var validacao = _consultaRepository.GetConsultasById(request.ConsultaId).Result;
            if (validacao == null) //agenda não existente
            {
                Exception exception = new Exception ("Agenda não existe");
                throw exception;
            }
            else if(validacao.DataAgendamento < DateTime.UtcNow)
            {
                Exception exception = new Exception("Data Agendamento Invalida");
                throw exception;
            }
            else
                await _consultaRepository.DeleteConsulta(request.ConsultaId); 
        }
    }
}
