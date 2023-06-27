﻿using IntMed.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Application.Commands.Consultas.Response
{
    public class CreateConsultaResponse
    {
        public string IdAgenda { get; set; }
        public string Id { get; set; }
        public string Dia { get; set; }
        public DateTime DataAgendamento { get; set; }
        public DateTime Horario { get; set; }
        public Medico Medico { get; set; }
    }
}
