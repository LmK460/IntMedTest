using IntMed.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Interfaces
{
    public interface IMedicoRepository
    {
        Task<Boolean> GetValidCRM(int crmId);

        Task<Medico> GetMedicobyId(int crmId);

        Task<Medico> InsertMedico(Medico Medico);
    }
}
