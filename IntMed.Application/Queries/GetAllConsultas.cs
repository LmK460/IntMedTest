using IntMed.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Queries
{
    public class GetAllConsultas:IRequest<ICollection <GetAllConsultasResponse>>
    {
    }
}
