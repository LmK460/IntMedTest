using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using MediatR;


namespace IntMed.Application.Commands.Medicos.CommandHandlers
{
    public class CreateMedicoHandler : IRequestHandler<CreateMedicoRequest,CreateMedicoResponse>
    {
        private readonly IMedicoRepository _medicoRepository;

        public CreateMedicoHandler(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }


        async Task<CreateMedicoResponse> IRequestHandler<CreateMedicoRequest, CreateMedicoResponse>.Handle(CreateMedicoRequest request, CancellationToken cancellationToken)
        {
            Medico result = new Medico();

            var aux = _medicoRepository.GetMedicobyId(request.CRM);

            if (_medicoRepository.GetValidCRM(request.CRM).Result == true) //se já existe um registro
            {
                return await Task.FromResult<CreateMedicoResponse>(null);
            }
            else
            {
                var medico = new Medico 
                { 
                    CRM = request.CRM,
                    Email = request.Email,
                    Nome = request.Nome
                };
                result = await _medicoRepository.InsertMedico(medico);
            }

            if(result.Id > 0)
            {
                return new CreateMedicoResponse
                { 
                    Id = result.Id ,
                    Email = request.Email,                                
                    CRM = request.CRM,
                    Nome=request.Nome 
                };
            }
            else
                return await Task.FromResult<CreateMedicoResponse>(null);

        }
    }
}
