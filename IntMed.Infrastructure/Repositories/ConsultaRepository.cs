using Dapper;
using IntMed.Application.Commands.Consultas.Response;
using IntMed.Application.DTOs;
using IntMed.Application.Interfaces;
using IntMed.Application.Queries;
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

        public async Task<ICollection<GetAllConsultasResponse>> GetAllConsultas()
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select con_id as id, a.med_id, horario, data_agendamento,"+
                            " m.med_id, m.nome, m.crm, m.email " +
                            "from consulta a "+
                            "inner join medicos m on a.med_id = m.med_id "+
                            "where horario > current_timestamp";

                ICollection<GetAllConsultasResponse> result = new List<GetAllConsultasResponse>();
                try
                {
                    var dr = await connection.ExecuteReaderAsync(sql);
                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {

                            var dto = new GetAllConsultasResponse
                            {
                                Id = (int)dr["id"],
                                Medico = new Medico
                                {
                                    Id = (int)dr["Id"],
                                    CRM = (int)dr["CRM"],
                                    Nome = dr["NOME"].ToString(),
                                    Email = dr["EMAIL"].ToString()
                                },
                                DataAgendamento = DateTime.Parse(dr["data_agendamento"].ToString()),
                                Horario = DateTime.Parse(dr["horario"].ToString())
                            };
                            result.Add(dto);

                        }
                    }

                        return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
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
                            Horario = await GetHorarioByConId(Id)
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

        public async Task<List<DateTime>> GetHorarioByConId(int Id)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select horario from horario where ag_id = @ID_P and status = 'A'";
                List<DateTime> result = new List<DateTime>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", Id);
                    var dr = await connection.ExecuteReaderAsync(sql, param);

                    

                        while (dr.Read())
                        {
                            if (dr.HasRows)
                            {
                                result.Add(DateTime.Parse(dr["horario"].ToString()));
                            }
                        } ;
                        //result.Add(DateTime.Parse(dr["horario"].ToString()));
                        //while (dr.Read()) ;
                        //{
                        //    result.Add(DateTime.Parse(dr["horario"].ToString()));
                        //}

                    
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
