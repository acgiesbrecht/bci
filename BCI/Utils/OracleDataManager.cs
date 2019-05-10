using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace BCI
{
    public class OracleDataManager
    {

        public List<XX_OPM_BCI_ITEMS_V> GetInventoryItemList()
        {            
                using (var dbConnection = GetConnection())
                {
                dbConnection.Open();                
                OracleGlobalization oracleGlobalization = dbConnection.GetSessionInfo();
                oracleGlobalization.Language = "LATIN AMERICAN SPANISH";
                dbConnection.SetSessionInfo(oracleGlobalization);
                return dbConnection.QueryAsync<XX_OPM_BCI_ITEMS_V>("Select * FROM APPS.XX_OPM_BCI_ITEMS_V ORDER BY DESCRIPCION_ITEM").Result.ToList();
                }            
        }

        public XX_OPM_BCI_ITEMS_V GetInventoryItemById(long id)
        {
            using (var dbConnection = GetConnection())
            {                
                return dbConnection.QueryFirstAsync<XX_OPM_BCI_ITEMS_V>("SELECT * FROM APPS.XX_OPM_BCI_ITEMS_V WHERE INVENTORY_ITEM_ID = " + id.ToString()).Result;
            }
        }

        public List<XX_OPM_BCI_CONTRATOS_V> GetContratosList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_CONTRATOS_V>("Select * FROM XX_OPM_BCI_CONTRATOS_V").ToList();
            }
        }

        public List<XX_OPM_BCI_ESTAB> GetEstabAPList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_ESTAB>(
                    " Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION AS Descripcion, RUC, COALESCE(ES_SOCIO, 'No') AS ES_SOCIO" +
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

        /*public List<string> GetLoteList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<string>("Select CODIGO AS Id, SIGNIFICADO AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_LOTE'").Result.ToList();
            }
        }*/

        public List<XX_OPM_BCI_PESADAS_ALL> GetPesadasPendientes()
        {
            return GetPesadas(" WHERE ESTADO = 'Pendiente' OR ESTADO = 'Completo'" +
                "ORDER BY GREATEST(COALESCE(FECHA_PESO_TARA, TO_DATE('1900-01-01', 'YYYY-MM-DD')), COALESCE(FECHA_PESO_BRUTO, TO_DATE('1900-01-01', 'YYYY-MM-DD'))) DESC");            
        }

        public List<XX_OPM_BCI_PESADAS_ALL> GetPesadasCerradas()
        {
            return GetPesadas(" WHERE ESTADO = 'Cerrado'" +
                "ORDER BY GREATEST(COALESCE(FECHA_PESO_TARA, TO_DATE('1900-01-01', 'YYYY-MM-DD')), COALESCE(FECHA_PESO_BRUTO, TO_DATE('1900-01-01', 'YYYY-MM-DD'))) DESC");
        }

        private List<XX_OPM_BCI_PESADAS_ALL> GetPesadas(String where)
        {
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                OracleGlobalization oracleGlobalization = dbConnection.GetSessionInfo();
                oracleGlobalization.Language = "LATIN AMERICAN SPANISH";
                dbConnection.SetSessionInfo(oracleGlobalization);
                return dbConnection.QueryAsync<XX_OPM_BCI_PESADAS_ALL>("SELECT p.*, COALESCE(v.ESTADO, 'Pendiente') AS ESTADO, COALESCE(v.DISPOSICION, 'Pendiente') AS DISPOSICION " +
                    "FROM XX_OPM_BCI_PESADAS_ALL p " +
                    "LEFT JOIN XX_OPM_BCI_PESADAS_ESTADOS_V v " +
                    "ON p.PESADA_ID = v.PESADA_ID" + where).Result.ToList();
            }
        }

        public XX_OPM_BCI_PESADAS_ALL GetPesadaByID(int id)
        {
            using (var dbConnection = GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("PESADA_ID", id);
                dbConnection.Open();
                OracleGlobalization oracleGlobalization = dbConnection.GetSessionInfo();
                oracleGlobalization.Language = "LATIN AMERICAN SPANISH";
                dbConnection.SetSessionInfo(oracleGlobalization);
                return dbConnection.QueryAsync<XX_OPM_BCI_PESADAS_ALL>("SELECT p.*, COALESCE(v.ESTADO, 'Pendiente') AS ESTADO, COALESCE(v.DISPOSICION, 'Pendiente') AS DISPOSICION " +
                    "FROM XX_OPM_BCI_PESADAS_ALL p " +
                    "LEFT JOIN XX_OPM_BCI_PESADAS_ESTADOS_V v " +
                    "ON p.PESADA_ID = v.PESADA_ID " +
                    "WHERE p.PESADA_ID = :PESADA_ID", param).Result.FirstOrDefault();
            }
        }

        public List<XX_OPM_BCI_CONTRATOS_V> GetContratoByEstablecimientoAndItem(XX_OPM_BCI_ESTAB estab, XX_OPM_BCI_ITEMS_V item)
        {
            using (var dbConnection = GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("ESTAB", estab.Id);
                param.Add("INVENTORY_ITEM_ID", item.INVENTORY_ITEM_ID);
                List<XX_OPM_BCI_CONTRATOS_V> result; 
                result = dbConnection.QueryAsync<XX_OPM_BCI_CONTRATOS_V>("select * from XX_OPM_BCI_CONTRATOS_V " +
                    "WHERE INVENTORY_ITEM_ID = :INVENTORY_ITEM_ID " +
                    "AND PROVEEDOR = :ESTAB " +
                    "AND SYSDATE BETWEEN TO_DATE(FECHA_INICIO_VIGENCIA, 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE(FECHA_FIN_VIGENCIA, 'YYYY/MM/DD HH24:MI:SS')" +
                    "", param).Result.ToList();
                if (result.Count == 0 && !estab.ES_SOCIO.Equals("Si"))
                {
                    result = dbConnection.QueryAsync<XX_OPM_BCI_CONTRATOS_V>("select * from XX_OPM_BCI_CONTRATOS_V " +
                    "WHERE INVENTORY_ITEM_ID = :INVENTORY_ITEM_ID " +
                    "AND PROVEEDOR IS NULL " +
                    "AND SYSDATE BETWEEN TO_DATE(FECHA_INICIO_VIGENCIA, 'YYYY/MM/DD HH24:MI:SS') AND TO_DATE(FECHA_FIN_VIGENCIA, 'YYYY/MM/DD HH24:MI:SS')" +
                    "", param).Result.ToList();
                }
                return result;
            }
        }

        public List<XX_OPM_BCI_LOTE> GetLotesAlgodonByEstablecimiento(string EstablecimientoCodigo)
        {
            using (var dbConnection = GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("ESTAB", EstablecimientoCodigo);
                return dbConnection.QueryAsync<XX_OPM_BCI_LOTE>("SELECT DISTINCT LOTE AS ID FROM XX_OPM_BCI_PESADAS_ALL WHERE LOTE LIKE CONCAT('%-', :ESTAB) AND LOTE LIKE CONCAT(TO_CHAR(SYSDATE, 'YY'), '%') ORDER BY LOTE", param).Result.ToList();
            }
        }

        public List<XX_OPM_BCI_LOTE> GetLotesDAE()
        {
            using (var dbConnection = GetConnection())
            {
                /*return dbConnection.QueryAsync<XX_OPM_BCI_LOTE>("SELECT DISTINCT LOTE AS ID FROM XX_OPM_BCI_PESADAS_ALL " +
                    " WHERE LENGTH (LOTE) > 11" +
                    " ORDER BY LOTE").Result.ToList();*/
                return dbConnection.QueryAsync<XX_OPM_BCI_LOTE>("Select CODIGO AS ID FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_LOTES_DAE'").Result.ToList();
            }
        }

        public XX_OPM_BCI_LOTE GetMaxLoteCurrentYear()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.QueryAsync<XX_OPM_BCI_LOTE>("SELECT COALESCE(MAX(LOTE), CONCAT(TO_CHAR(SYSDATE, 'YY'), '-000-0000')) AS ID " +
                    "FROM XX_OPM_BCI_PESADAS_ALL " +
                    "WHERE LOTE LIKE CONCAT(TO_CHAR(SYSDATE, 'YY'), '%')").Result.Single();
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
            param.Add("FECHA_PESO_TARA", pesada.FECHA_PESO_TARA);
            param.Add("MODO_PESO_TARA", pesada.MODO_PESO_TARA);
            param.Add("PESO_ORIGEN", pesada.PESO_ORIGEN);
            param.Add("NRO_NOTA_REMISION", pesada.NRO_NOTA_REMISION);
            param.Add("CREATED_BY", pesada.CREATED_BY);
            param.Add("CREATION_DATE", pesada.CREATION_DATE);
            param.Add("LAST_UPDATED_BY", pesada.LAST_UPDATED_BY);
            param.Add("LAST_UPDATE_DATE", pesada.LAST_UPDATE_DATE);
            string sql = "INSERT INTO XX_OPM_BCI_PESADAS_ALL (ORG_ID, ORGANIZATION_ID, TIPO_ACTIVIDAD, " +
                "INVENTORY_ITEM_ID, PUNTO_DESCARGA, ESTABLECIMIENTO, " +
                "OBSERVACIONES, NRO_BASCULA, LOTE, CONTRATO, MATRICULA, " +
                "PESO_BRUTO, MODO_PESO_BRUTO, FECHA_PESO_BRUTO, " +
                "PESO_TARA, MODO_PESO_TARA, FECHA_PESO_TARA, PESO_ORIGEN, NRO_NOTA_REMISION, " +
                "CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE) " +
                "Values (:ORG_ID, :ORGANIZATION_ID, :TIPO_ACTIVIDAD, " +
                ":INVENTORY_ITEM_ID, :PUNTO_DESCARGA, :ESTABLECIMIENTO, " +
                ":OBSERVACIONES, :NRO_BASCULA, :LOTE, :CONTRATO, :MATRICULA, " +
                ":PESO_BRUTO, :MODO_PESO_BRUTO, :FECHA_PESO_BRUTO, " +
                ":PESO_TARA, :MODO_PESO_TARA, :FECHA_PESO_TARA, :PESO_ORIGEN, :NRO_NOTA_REMISION, " +
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
            param.Add("PUNTO_DESCARGA", pesada.PUNTO_DESCARGA);
            param.Add("ESTABLECIMIENTO", pesada.ESTABLECIMIENTO);
            param.Add("OBSERVACIONES", pesada.OBSERVACIONES);            
            param.Add("LOTE", pesada.LOTE);
            param.Add("CONTRATO", pesada.CONTRATO);
            param.Add("MATRICULA", pesada.MATRICULA);
            param.Add("PESO_BRUTO", pesada.PESO_BRUTO);
            param.Add("MODO_PESO_BRUTO", pesada.MODO_PESO_BRUTO);
            param.Add("FECHA_PESO_BRUTO", pesada.FECHA_PESO_BRUTO);
            param.Add("PESO_TARA", pesada.PESO_TARA);
            param.Add("MODO_PESO_TARA", pesada.MODO_PESO_TARA);
            param.Add("FECHA_PESO_TARA", pesada.FECHA_PESO_TARA);
            param.Add("PESO_ORIGEN", pesada.PESO_ORIGEN);
            param.Add("NRO_NOTA_REMISION", pesada.NRO_NOTA_REMISION);
            param.Add("LAST_UPDATED_BY", pesada.LAST_UPDATED_BY);
            param.Add("LAST_UPDATE_DATE", pesada.LAST_UPDATE_DATE);
            string sql = "UPDATE XX_OPM_BCI_PESADAS_ALL SET " +
                "PUNTO_DESCARGA = :PUNTO_DESCARGA, ESTABLECIMIENTO = :ESTABLECIMIENTO, " +
                "LOTE = :LOTE, CONTRATO = :CONTRATO, MATRICULA = :MATRICULA, " +
                "PESO_BRUTO = :PESO_BRUTO, MODO_PESO_BRUTO = :MODO_PESO_BRUTO, FECHA_PESO_BRUTO = :FECHA_PESO_BRUTO, " +
                "PESO_TARA = :PESO_TARA, MODO_PESO_TARA = :MODO_PESO_TARA, " +
                "FECHA_PESO_TARA = :FECHA_PESO_TARA, PESO_ORIGEN = :PESO_ORIGEN, NRO_NOTA_REMISION = :NRO_NOTA_REMISION, " +
                "OBSERVACIONES = :OBSERVACIONES, " +
                "LAST_UPDATED_BY = :LAST_UPDATED_BY, LAST_UPDATE_DATE = :LAST_UPDATE_DATE" +
                " WHERE PESADA_ID = :PESADA_ID";
            using (var dbConnection = GetConnection())
            {
                var affectedRows = await dbConnection.ExecuteAsync(sql, param);
            }
        }

        public OracleConnection GetConnection()
        {
            const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=orca.chortitzer.com.py)(PORT=1522)) (CONNECT_DATA=(SERVICE_NAME=TEST))); User Id=XXBCI;Password=XXBCI;";
            var connection = new OracleConnection(connectionString);            
            return connection;
        }
        
        //alter session set nls_language = 'LATIN AMERICAN SPANISH';

    }
}
