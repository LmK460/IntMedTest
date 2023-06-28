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
    internal class GetAllConsultasHander : IRequestHandler<GetAllConsultas, ICollection<GetAllConsultasResponse>>
    {
        private readonly IConsultaRepository _consultaRepository;

        public GetAllConsultasHander(IConsultaRepository consultaRepository)
        {
            _consultaRepository = consultaRepository;
        }



        public async Task<ICollection<GetAllConsultasResponse>> Handle(GetAllConsultas request, CancellationToken cancellationToken)
        {
            return await _consultaRepository.GetAllConsultas();
        }
    }
}
