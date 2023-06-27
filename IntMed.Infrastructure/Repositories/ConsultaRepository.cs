using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using IntMed.Infrastructure.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Infrastructure.Repositories
{
    

    public class ConsultaRepository : IConsultaRepository
    {

        public IDataBaseConnectionFactory DatabaseConnectionFactory { get; }

        public ConsultaRepository(IDataBaseConnectionFactory databaseConnectionFactory)
        {
            DatabaseConnectionFactory = databaseConnectionFactory;
        }

        public Task<CreateConsultaResponse> CreateConsulta(CreateConsultaResponse consulta)
        {
            throw new NotImplementedException();
        }

        public Task DeleteConsulta(string consultaId)
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
