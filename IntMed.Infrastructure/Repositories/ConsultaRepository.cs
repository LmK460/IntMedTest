using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Infrastructure.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        public Task<Consulta> CreateConsulta(Consulta consulta)
        {
            throw new NotImplementedException();
        }

        public Task<Consulta> DeleteConsulta(Consulta consulta)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Consulta>> GetAllConsultas()
        {
            throw new NotImplementedException();
        }

        public Task<Consulta> GetConsultasById(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
