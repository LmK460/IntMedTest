using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntMed.Domain.Models;

namespace IntMed.Application.Interfaces
{
    public interface IConsultaRepository
    {
        Task<Consulta> GetConsultasById(string Id);
        Task<ICollection<Consulta>> GetAllConsultas();

        Task<Consulta> CreateConsulta(Consulta consulta);
        Task<Consulta> DeleteConsulta(Consulta consulta);

    }
}
