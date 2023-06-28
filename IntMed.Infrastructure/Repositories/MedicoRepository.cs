using Dapper;
using IntMed.Application.Interfaces;
using IntMed.Domain.Models;
using IntMed.Infrastructure.Factory;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Infrastructure.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        public IDataBaseConnectionFactory DatabaseConnectionFactory { get; }

        public MedicoRepository(IDataBaseConnectionFactory databaseConnectionFactory)
        {
            DatabaseConnectionFactory = databaseConnectionFactory;
        }
        public async Task<bool> GetValidCRM(int crmId)
        {
            using(var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                try
                {
                    var sql = "select GETVALIDCRM(@CRM_P)";
                    var p = new DynamicParameters();
                    p.Add("@CRM_P", crmId);
                    return await connection.ExecuteScalarAsync<bool>(sql, p);
                }
                catch (Exception ex)
                {
                    return false;
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
                    var dr = await connection.ExecuteReaderAsync(sql, param) ;

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

        public async Task<Medico> InsertMedico(Medico Medico)
        {
            using (var connection = await DatabaseConnectionFactory.GetConnectionFactoryAsync())
            {
                try
                {
                    var sql = "call CreateMedico(@NOME_P,@CRM_P,@EMAIL_P,@ID_OUT) ";
                    var param = new DynamicParameters();
                    param.Add("@NOME_P", Medico.Nome);
                    param.Add("@CRM_P", Medico.CRM);
                    param.Add("@EMAIL_P", Medico.Email);
                    param.Add("@ID_OUT", Medico.Id);

                    var medico = await connection.ExecuteScalarAsync<int>(sql, param);
                    if(medico > 0)
                    {
                        Medico.Id = medico;
                    }
                    return Medico;
                }
                catch (Exception ex)
                {
                    return new Medico();
                }
            }
        }
    }
}
