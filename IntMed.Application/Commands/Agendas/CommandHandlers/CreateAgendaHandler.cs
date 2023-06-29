using IntMed.Application.Commands.Agendas.Request;
using IntMed.Application.Commands.Agendas.Response;
using IntMed.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Agendas.CommandHandlers
{
    public class CreateAgendaHandler : IRequestHandler<CreateAgendaRequest, CreateAgendaResponse>
    {
        private readonly IAgendaRepository _agendaRepository;

        public CreateAgendaHandler(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }


        async Task<CreateAgendaResponse> IRequestHandler<CreateAgendaRequest, CreateAgendaResponse>.Handle(CreateAgendaRequest request, CancellationToken cancellationToken)
        {

            if(request.Dia < DateTime.Now)
            {
                Exception exception = new Exception("Periodo Invalido");
                throw exception;
            }
            //valida horarios
            foreach (var item in request.horario)
            {
                if( int.Parse(item.Split(':')[0]) > 24 || int.Parse(item.Split(':')[1]) > 59){
                    return new CreateAgendaResponse();
                }
            }

            var agendaDtoDia = await _agendaRepository.GetAgendaByMedId(request.Med_id, request.Dia);

            if (agendaDtoDia.AgendaId <= 0)
            {
                //faz insercao
                CreateAgendaRequest createAgendaRequest = new CreateAgendaRequest
                {
                    Dia = request.Dia,
                    Med_id = request.Med_id,
                    horario = request.horario
                };
                var result = await _agendaRepository.CreateAgenda(createAgendaRequest);
                return new CreateAgendaResponse
                {
                    AgendaId = result.AgendaId,
                    Medico = result.Medico,
                    Dia = result.Dia
                };
            }
            else
            {
                Exception exception = new Exception("Não foi possivel inserir o registro");
                throw exception;
            }
        }
    }
}
