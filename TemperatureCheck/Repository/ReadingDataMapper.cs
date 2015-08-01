using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using TemperatureCheck.Interfaces;
using TemperatureCheck.Models;

namespace TemperatureCheck.Repository
{
    public class ReadingDataMapper : IDataMapper
    {
        private readonly SqlConnection _sqlConnection;
        private readonly string _sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public ReadingDataMapper()
        {
            _sqlConnection = new SqlConnection(_sqlConnectionString);
        }
        public bool ReadingInsert(float ourTemperature)
        {
            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                // build our SQL Query
                string ourSql = "INSERT INTO [rpi_roomtemp].[Reading] ([ReadingTemp],[ReadingTime]) VALUES (@Temperature, SYSDATETIME())";
                
                var cmd = new SqlCommand(ourSql)
                {
                    Connection = _sqlConnection
                };

                cmd.Parameters.AddWithValue("@Temperature", ourTemperature);

                cmd.ExecuteScalar();
                _sqlConnection.Close();

                return true;

            }
            catch (Exception storeInicidentSqlEx)
            {
                Debug.WriteLine(storeInicidentSqlEx.ToString());
                _sqlConnection.Close();
                return false;
            }
        }

        public List<SingleTempResult> ReadingGet(int count, string sortOrder)
        {
            var ourReturnList = new List<SingleTempResult>();
            
            try
            {

               using (_sqlConnection)
                {
                    _sqlConnection.Open();
                    //
                    // Create new SqlCommand object.
                    //
                    using (
                        SqlCommand command =
                            new SqlCommand(
                                "SELECT TOP " + count + " [ReadingTemp],[ReadingTime] FROM[rpi_roomtemp].[rpi_roomtemp].[Reading] ORDER BY ReadingID " + sortOrder,
                                _sqlConnection))
                    {
                        //
                        // Invoke ExecuteReader method.
                        //
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var ourSingleResult = new SingleTempResult();
                            //Debug.WriteLine("VALUE IS " + reader.GetFloat(0));

                            ourSingleResult.ReadingTemp = (float) reader.GetDouble(0);
                            ourSingleResult.ReadingTime = reader.GetDateTime(1);

                            ourReturnList.Add(ourSingleResult);

                        }
                    }
                }

                _sqlConnection.Close();

            }
            catch (SqlException ex)
            {
                Debug.Write(ex.ToString());
            }

            return ourReturnList;
        }
    }
}