using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Medicos
{
    public class CreateMedicoRequest : IRequest<CreateMedicoResponse>
    {
        public string Nome { get; set; }
        public int CRM { get; set; }
        public string Email { get; set; }
    }
}
