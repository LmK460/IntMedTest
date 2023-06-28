using Dapper;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.DTOs;
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

        public async Task DeleteConsulta(int consultaId)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "DELETE from consulta where con_id = @ID_P";
                Medico result = new Medico();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", consultaId);
                    var cont = await connection.ExecuteScalarAsync<int>(sql, param);

                }
                catch (Exception ex)
                {

                }
            }
        }

        public Task<ICollection<Consulta>> GetAllConsultas()
        {
            throw new NotImplementedException();
        }

        public async Task<ConsultaDTO> GetConsultasById(int Id)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select con_id as id, ag_id, med_id, horario, data_agendamento from consulta where con_id = @ID_P";
                ConsultaDTO result = new ConsultaDTO();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", Id);
                    var dr = await connection.ExecuteReaderAsync(sql, param);

                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = new ConsultaDTO
                        {
                            Id = (int)dr["id"],
                            AgendaId = (int)dr["ag_id"],
                            MedicoId = (int)dr["med_id"],
                            DataAgendamento = DateTime.Parse(dr["data_agendamento"].ToString()),
                            Horario = DateTime.Parse(dr["horario"].ToString())
                        };

                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
