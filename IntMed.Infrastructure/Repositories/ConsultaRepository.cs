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

        public async Task<CreateConsultaResponse> CreateConsulta(CreateConsultaResponse consulta)
        {

            int hr_id = await GetHorarioByAgId(consulta.IdAgenda, consulta.Horario);
            if (hr_id>0) //horario disponivel
            {
                using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
                {
                    var sql = "call createconsulta(@AG_ID,@HORA,@HR_ID,@ID_OUT) ";
                    var update = "update horario set status = 'F', data_modificacao = current_timestamp where hor_id = @HR_ID";
                    try
                    {
                        var param = new DynamicParameters();
                        param.Add("@AG_ID", consulta.IdAgenda);
                        param.Add("@HORA", consulta.Horario);
                        param.Add("@HR_ID", hr_id);
                        param.Add("@ID_OUT", consulta.Id);
                        var cont = await connection.ExecuteScalarAsync<int>(sql, param);
                        if (cont > 0)
                        {
                            var paramUpdate = param = new DynamicParameters();
                            paramUpdate.Add("@HR_ID", hr_id);

                            await connection.ExecuteScalarAsync<int>(update, paramUpdate);

                            return await GetConsultasById(cont);
                        }
                        else
                            return new CreateConsultaResponse();
 
                    }
                    catch (Exception ex)
                    {
                        return new CreateConsultaResponse();
                    }
                }
            }
            else
                return new CreateConsultaResponse();
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


        public async Task<CreateConsultaResponse> GetConsultasById(int Id)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select con_id as id, ag_id, m.med_id, horario, data_agendamento, m.nome, m.crm, m.email from consulta a " +
                    "inner join medicos m on a.med_id = m.med_id " +
                    " where con_id = @ID_P";
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", Id);
                    var dr = await connection.ExecuteReaderAsync(sql, param);
                    var result = new CreateConsultaResponse();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        var med_id = (int)dr["med_id"];
                        result = new CreateConsultaResponse
                        {
                            Id = (int)dr["id"],
                            IdAgenda = (int)dr["ag_id"],
                            DataAgendamento = DateTime.Parse(dr["data_agendamento"].ToString()),
                            Horario = DateTime.Parse(dr["horario"].ToString()),
                            Medico = new Medico
                            {
                                CRM = (int)dr["crm"],
                                Email = (string)dr["email"],
                                Id = (int)dr["med_id"],
                                Nome = (string)dr["nome"]
                            },
                            Dia = DateTime.Parse(dr["horario"].ToString()).ToShortDateString()
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



        public async Task<int> GetHorarioByAgId(int agendaId, DateTime data)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select hor_id from horario where ag_id = @ID_P and status = 'A' and horario = @DATA_P LIMIT 1";
                List<DateTime> result = new List<DateTime>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", agendaId);
                    param.Add("@DATA_P", data);
                    int dr = await connection.ExecuteScalarAsync<int>(sql, param);

                    return dr;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }


        public async Task<Medico> GetMedicobyId(int crmId)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select med_id as ID,email as EMAIL, crm as CRM,nome as NOME from medicos where med_id = @ID_P";
                Medico result = new Medico();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", crmId);
                    var dr = await connection.ExecuteReaderAsync(sql, param);

                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = new Medico
                        {
                            Id = (int)dr["Id"],
                            CRM = (int)dr["CRM"],
                            Nome = dr["NOME"].ToString(),
                            Email = dr["EMAIL"].ToString()
                        };

                    }

                    return result;
                    //}

                }
                catch (Exception ex)
                {
                    return new Medico();
                }
            }
        }
    }
}
