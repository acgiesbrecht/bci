using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinBascula.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Oracle.ManagedDataAccess.Client;

namespace CinBascula
{
    public class OracleDataManager
    {

        public List<XX_OPM_BCI_ITEMS_V> GetInventoryItemList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_ITEMS_V>("Select * FROM APPS.XX_OPM_BCI_ITEMS_V ORDER BY DESCRIPCION_ITEM").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_CONTRATOS_V> GetContratosList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_CONTRATOS_V>("Select * FROM XX_OPM_BCI_CONTRATOS_V").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_ESTAB> GetEstabAPList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_ESTAB>(
                    " Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AP_V ORDER BY RAZON_SOCIAL").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_ESTAB> GetEstabARList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_ESTAB>(
                    " Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AR_V ORDER BY RAZON_SOCIAL").Result.ToList();
            }
        }

        /*public List<XX_OPM_BCI_ESTAB> GetEstabAllList()
        {   
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_ESTAB>("Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC, 1 AS TipoActividad" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AP_V" +
                    " UNION ALL" +
                    " Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC, 2 AS TipoActividad" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AR_V").AsList();
            }
        }*/

        public List<XX_OPM_BCI_ORGS_COMPLEJO> GetOrgsComplejoList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_ORGS_COMPLEJO>("Select CODIGO AS Id, SIGNIFICADO AS Tag, DESCRIPCION AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_ORGS_COMPLEJO'").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_PUNTO_OPERACION> GetPuntoDescargaList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_PUNTO_OPERACION>("Select CODIGO AS Id, SIGNIFICADO AS Description, TAG AS Tag FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_PUNTO_DESCARGA'").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_PUNTO_OPERACION> GetPuntoCargaList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_PUNTO_OPERACION>("Select CODIGO AS Id, SIGNIFICADO AS Description, TAG AS Tag FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_PUNTO_CARGA'").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_TIPO_ACTIVIDAD> GetTipoActividadList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_TIPO_ACTIVIDAD>("Select CODIGO AS Id, SIGNIFICADO AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_TIPO_ACTIVIDAD'").Result.ToList();
            }
        }

        public List<string> GetLoteList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<string>("Select CODIGO AS Id, SIGNIFICADO AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_LOTE'").Result.ToList();
            }
        }

        public List<XX_OPM_BCI_PESADAS_ALL> GetPesadas()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_PESADAS_ALL>("Select * from XX_OPM_BCI_PESADAS_ALL ORDER BY PESADA_ID DESC").Result.ToList();
            }
        }

        public List<string> GetLotesByEstablecimiento(string EstablecimientoCodigo)
        {
            using (var dbConnection = GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("ESTAB", EstablecimientoCodigo);
                return dbConnection.QueryAsync<string>("SELECT DISTINCT LOTE FROM XX_OPM_BCI_PESADAS_ALL WHERE LOTE LIKE CONCAT('%-', :ESTAB) AND LOTE LIKE CONCAT(TO_CHAR(SYSDATE, 'YY'), '%') ORDER BY LOTE").Result.ToList();
            }
        }

        public async void insertNewPesada(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            var param = new DynamicParameters();
            param.Add("ORG_ID", pesada.ORG_ID);
            param.Add("ORGANIZATION_ID", pesada.ORGANIZATION_ID);
            param.Add("TIPO_ACTIVIDAD", pesada.TIPO_ACTIVIDAD);
            param.Add("INVENTORY_ITEM_ID", pesada.INVENTORY_ITEM_ID);
            param.Add("PUNTO_DESCARGA", pesada.PUNTO_DESCARGA);
            param.Add("ESTABLECIMIENTO", pesada.ESTABLECIMIENTO);
            param.Add("OBSERVACIONES", pesada.OBSERVACIONES);
            param.Add("NRO_BASCULA", pesada.NRO_BASCULA);
            param.Add("LOTE", pesada.LOTE);
            param.Add("CONTRATO", pesada.CONTRATO);
            param.Add("MATRICULA", pesada.MATRICULA);
            param.Add("PESO_BRUTO", pesada.PESO_BRUTO);
            param.Add("MODO_PESO_BRUTO", pesada.MODO_PESO_BRUTO);
            param.Add("FECHA_PESO_BRUTO", pesada.FECHA_PESO_BRUTO);
            param.Add("PESO_TARA", pesada.PESO_TARA);
            param.Add("MODO_PESO_TARA", pesada.MODO_PESO_TARA);
            param.Add("FECHA_PESO_TARA", pesada.FECHA_PESO_TARA);
            param.Add("CREATED_BY", pesada.CREATED_BY);
            param.Add("CREATION_DATE", pesada.CREATION_DATE);
            param.Add("LAST_UPDATED_BY", pesada.LAST_UPDATED_BY);
            param.Add("LAST_UPDATE_DATE", pesada.LAST_UPDATE_DATE);
            string sql = "INSERT INTO XX_OPM_BCI_PESADAS_ALL (ORG_ID, ORGANIZATION_ID, TIPO_ACTIVIDAD, " +
                "INVENTORY_ITEM_ID, PUNTO_DESCARGA, ESTABLECIMIENTO, " +
                "OBSERVACIONES, NRO_BASCULA, LOTE, CONTRATO, MATRICULA, " +
                "PESO_BRUTO, MODO_PESO_BRUTO, FECHA_PESO_BRUTO, " +
                "PESO_TARA, MODO_PESO_TARA, FECHA_PESO_TARA, " +
                "CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE) " +
                "Values (:ORG_ID, :ORGANIZATION_ID, :TIPO_ACTIVIDAD, " +
                ":INVENTORY_ITEM_ID, :PUNTO_DESCARGA, :ESTABLECIMIENTO, " +
                ":OBSERVACIONES, :NRO_BASCULA, :LOTE, :CONTRATO, :MATRICULA, " +
                ":PESO_BRUTO, :MODO_PESO_BRUTO, :FECHA_PESO_BRUTO, " +
                ":PESO_TARA, :MODO_PESO_TARA, :FECHA_PESO_TARA, " +
                ":CREATED_BY, :CREATION_DATE, :LAST_UPDATED_BY, :LAST_UPDATE_DATE)";
            using (var dbConnection = GetConnection())
            {                
                var affectedRows = await dbConnection.ExecuteAsync(sql, param);
            }
        }

        public async void updatePesada(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            var param = new DynamicParameters();
            param.Add("PESADA_ID", pesada.PESADA_ID);
            param.Add("PESO_BRUTO", pesada.PESO_BRUTO);
            param.Add("MODO_PESO_BRUTO", pesada.MODO_PESO_BRUTO);
            param.Add("FECHA_PESO_BRUTO", pesada.FECHA_PESO_BRUTO);
            param.Add("PESO_TARA", pesada.PESO_TARA);
            param.Add("MODO_PESO_TARA", pesada.MODO_PESO_TARA);
            param.Add("FECHA_PESO_TARA", pesada.FECHA_PESO_TARA);            
            param.Add("LAST_UPDATED_BY", pesada.LAST_UPDATED_BY);
            param.Add("LAST_UPDATE_DATE", pesada.LAST_UPDATE_DATE);
            string sql = "UPDATE XX_OPM_BCI_PESADAS_ALL SET " +
                "PESO_BRUTO = :PESO_BRUTO, MODO_PESO_BRUTO = :MODO_PESO_BRUTO, FECHA_PESO_BRUTO = :FECHA_PESO_BRUTO, " +
                "PESO_TARA = :PESO_TARA, MODO_PESO_TARA = :MODO_PESO_TARA, FECHA_PESO_TARA = :FECHA_PESO_TARA, " +
                "LAST_UPDATED_BY = :LAST_UPDATED_BY, LAST_UPDATE_DATE = :LAST_UPDATE_DATE" +
                " WHERE PESADA_ID = :PESADA_ID";
            using (var dbConnection = GetConnection())
            {
                var affectedRows = await dbConnection.ExecuteAsync(sql, param);
            }
        }

        public IDbConnection GetConnection()
        {
            const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=orca.chortitzer.com.py)(PORT=1522)) (CONNECT_DATA=(SERVICE_NAME=TEST))); User Id=XXBCI;Password=XXBCI;";
            var connection = new OracleConnection(connectionString);
            return connection;
        }

    }
}
