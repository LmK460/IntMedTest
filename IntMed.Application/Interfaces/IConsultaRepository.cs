using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Domain.Models;

namespace IntMed.Application.Interfaces
{
    public interface IConsultaRepository
    {
        Task<Consulta> GetConsultasById(string Id);
        Task<ICollection<Consulta>> GetAllConsultas();

        Task<CreateConsultaResponse> CreateConsulta(CreateConsultaResponse consulta);
        Task DeleteConsulta(string consultaId);

    }
}
