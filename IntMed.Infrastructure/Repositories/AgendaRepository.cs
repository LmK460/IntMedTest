using Dapper;
using IntMed.Application.Commands.Agendas.Request;
using IntMed.Application.DTOs;
using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using IntMed.Infrastructure.Factory;


namespace IntMed.Infrastructure.Repositories
{
    public class AgendaRepository : IAgendaRepository
    {
        public IDataBaseConnectionFactory DatabaseConnectionFactory { get; }

        public AgendaRepository(IDataBaseConnectionFactory databaseConnectionFactory)
        {
            DatabaseConnectionFactory = databaseConnectionFactory;
        }


        public async Task<Agenda> CreateAgenda(CreateAgendaRequest agenda)
        {

            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                var result = new Agenda();
                
                try
                {
                    var sql = "call CreateAgenda(@MED_ID,@CRM_P,@ID_OUT) ";
                    var param = new DynamicParameters();
                    param.Add("@MED_ID", agenda.Med_id);
                    param.Add("@DATA_P", agenda.Dia);
                    param.Add("@ID_OUT", result.AgendaId);
                    var agId = await connection.ExecuteScalarAsync<int>(sql, param);
                    if (agId > 0)
                    {
                        result.AgendaId = agId;
                        result.Dia = agenda.Dia;
                        result.Medico = new Medico(await GetMedicobyId(agenda.Med_id));
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }

        }

        public async Task<AgendaDTO> GetAgendaById(int Id)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select ag_id as id, med_id, data_agenda, data_modificacao from agenda where ag_id = @ID_P";
                AgendaDTO result = new AgendaDTO();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", Id);
                    var dr = await connection.ExecuteReaderAsync(sql, param);

                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = new AgendaDTO
                        {
                            AgendaId = (int)dr["ag_id"],
                            MedicoId = (int)dr["med_id"],
                            Dia = DateTime.Parse(dr["data_agendamento"].ToString()),
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

        public async Task<AgendaDTO> GetAgendaByMedId(int Id,DateTime dateAgenda)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select ag_id as id, med_id, data_agenda, data_modificacao from agenda where med_id = @ID_P and to_char(data_agenda,'DD/MM/YYYY') = @DATA_P";
                AgendaDTO result = new AgendaDTO();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@ID_P", Id);
                    param.Add("@DATA_P", dateAgenda.ToShortDateString());
                    var dr = await connection.ExecuteReaderAsync(sql, param);

                    if (dr.HasRows)
                    {
                        dr.Read();
                        result = new AgendaDTO
                        {
                            AgendaId = (int)dr["ag_id"],
                            MedicoId = (int)dr["med_id"],
                            Dia = DateTime.Parse(dr["data_agenda"].ToString()),
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

        public async Task<ICollection<Agenda>> GetAllAgendas()
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                string sql = "select a.ag_id,m.med_id,a.data_modificacao,a.data_agenda, m.nome, m.crm, m.email from agenda a "+
                             "inner join medicos m on a.med_id = m.med_id "+
                             "group by a.ag_id,m.med_id "+
                             "having(select count(1) from horario as hor where hor.ag_id = a.ag_id and hor.status = 'A') >= 1";
                Agenda dto = new Agenda();
                ICollection<Agenda> result = new List<Agenda>();
                try
                {
                    var dr = await connection.ExecuteReaderAsync(sql);

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            dto = new Agenda
                            {
                                AgendaId = (int)dr["ag_id"],
                                Dia = DateTime.Parse(dr["data_agenda"].ToString()),
                                Medico = new Medico
                                {
                                    CRM = (int)dr["crm"],
                                    Email = (string)dr["email"],
                                    Id = (int)dr["med_id"],
                                    Nome = (string)dr["nome"]
                                }
                            };
                            dto.Horarios = await GetHorarioByAgId(dto.AgendaId);
                            result.Add(dto);
                        }
                    };

                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public async Task<List<DateTime>> GetHorarioByAgId(int Id)
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
                    };


                    return result;
                }
                catch (Exception ex)
                {
                    return null;
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
