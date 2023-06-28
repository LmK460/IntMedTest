using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.DTOs;
using IntMed.Domain.Models;

namespace IntMed.Application.Interfaces
{
    public interface IConsultaRepository
    {
        Task<ConsultaDTO> GetConsultasById(int Id);
        Task<ICollection<Consulta>> GetAllConsultas();

        Task<CreateConsultaResponse> CreateConsulta(CreateConsultaResponse consulta);
        Task DeleteConsulta(int consultaId);
        Task<List<DateTime>> GetHorarioByConId(int Id);
    }
}
