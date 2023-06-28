using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Queries.QueryHandles
{
    internal class GetAllAgendasHandler : IRequestHandler<GetAllAgendas, ICollection<Agenda>>
    {
        private readonly IAgendaRepository _agendaRepository;

        public GetAllAgendasHandler(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;
        }

        public async Task<ICollection<Agenda>> Handle(GetAllAgendas request, CancellationToken cancellationToken)
        {
            return await _agendaRepository.GetAllAgendas();
        }
    }
}
