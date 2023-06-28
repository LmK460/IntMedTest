﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Domain.Models
{
    public class Agenda
    {
        public int AgendaId { get; set; }
        public Medico Medico { get; set; }
        public DateTime Dia { get; set; }

        public List<DateTime> Horarios { get ; set; }
    }
}
