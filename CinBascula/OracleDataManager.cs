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
    class OracleDataManager
    {

        public List<XX_OPM_BCI_ITEMS_V> GetInventoryItemList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_ITEMS_V>("Select * FROM APPS.XX_OPM_BCI_ITEMS_V").AsList();
            }
        }

        public List<XX_OPM_BCI_CONTRATOS_V> GetContratosList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_CONTRATOS_V>("Select * FROM XX_OPM_BCI_CONTRATOS_V").AsList();
            }
        }

        

        public List<XX_OPM_BCI_ESTAB> GetEstabAllList()
        {   
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_ESTAB>("Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC, 'AP' AS ApAr" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AP_V" +
                    " UNION ALL" +
                    " Select CODIGO AS Id, RAZON_SOCIAL AS RazonSocial, DESCRIPCION as Descripcion, RUC, 'AR' AS ApAr" +
                    " FROM APPS.XX_OPM_BCI_ESTAB_AR_V").AsList();
            }
        }

        public List<XX_OPM_BCI_ORGS_COMPLEJO> GetOrgsComplejoList()
        {
            using (var dbConnection = GetConnection())
            {
                return dbConnection.Query<XX_OPM_BCI_ORGS_COMPLEJO>("Select CODIGO AS Id, SIGNIFICADO AS Tag, DESCRIPCION AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_ORGS_COMPLEJO'").AsList();
            }
        }

        public ObservableCollection<XX_OPM_BCI_PUNTO_DESCARGA> GetPuntoDescargaList()
        {
            using (var dbConnection = GetConnection())
            {
                return new ObservableCollection<XX_OPM_BCI_PUNTO_DESCARGA>(dbConnection.Query<XX_OPM_BCI_PUNTO_DESCARGA>("Select CODIGO AS Id, SIGNIFICADO AS Description, TAG AS Tag FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_PUNTO_DESCARGA'").AsList());
            }
        }

        public ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD> GetTipoActividadList()
        {
            using (var dbConnection = GetConnection())
            {
                return new ObservableCollection<XX_OPM_BCI_TIPO_ACTIVIDAD>(dbConnection.Query<XX_OPM_BCI_TIPO_ACTIVIDAD>("Select CODIGO AS Id, SIGNIFICADO AS Description FROM APPS.XX_OPM_BCI_LOOKUPS_V WHERE LOOKUP_TYPE = 'XX_OPM_BCI_TIPO_ACTIVIDAD'").AsList());
            }
        }

        public List<XX_OPM_BCI_PESADAS_ALL> GetPesadas()
        {
            using (var dbConnection = GetConnection())
            {                
                return dbConnection.Query<XX_OPM_BCI_PESADAS_ALL>("Select * from XX_OPM_BCI_PESADAS_ALL").AsList();                                
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
