using IntMed.Application.Commands.Consulta;
using IntMed.Application.Commands.Consultas.Requests;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using MediatR;
using System;

namespace IntMed.Application.Commands.Consulta.CommandHandlers
{
    public class CreateConsultaHandler : IRequestHandler<CreateConsultaRequest, CreateConsultaResponse>
    {

        private readonly IConsultaRepository _consultaRepository;

        public CreateConsultaHandler(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }

        public async Task<CreateConsultaResponse> Handle(CreateConsultaRequest request, CancellationToken cancellationToken)
        {
            var hora = int.Parse(request.Horario.Split(':')[0]);
            var minuto = int.Parse(request.Horario.Split(':')[1]);
            var newConsulta = new CreateConsultaResponse
            {

                Horario = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hora, minuto, 0),
                IdAgenda = request.AgendaId

            };

            return await _consultaRepository.CreateConsulta(newConsulta);
        }
    }
}
