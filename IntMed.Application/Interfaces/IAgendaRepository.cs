using IntMed.Application.Commands.Agendas.Request;
using IntMed.Application.DTOs;
using IntMed.Domain.Models;


namespace IntMed.Application.Interfaces
{
    public interface IAgendaRepository
    {
        Task<AgendaDTO> GetAgendaById(int Id);
        Task<ICollection<Agenda>> GetAllAgendas();
        Task<Agenda> CreateAgenda(CreateAgendaRequest agenda);
        Task<AgendaDTO> GetAgendaByMedId(int Id, DateTime dataAgenda);

        Task<List<DateTime>> GetHorarioByAgId(int Id);
    }
}
