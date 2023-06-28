using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Domain.Models
{
    public class Medico
    {
        private Medico medico;

        public Medico(Medico medico)
        {
            this.medico = medico;
        }

        public Medico()
        {
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int CRM { get; set; }
        public string Email { get; set; }
    }
}
