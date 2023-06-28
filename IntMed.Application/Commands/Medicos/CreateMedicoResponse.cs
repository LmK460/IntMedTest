using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Medicos
{
    public class CreateMedicoResponse :IRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CRM { get; set; }
        public string Email { get; set; }
    }
}
