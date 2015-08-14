using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using TemperatureCheck.Interfaces;
using TemperatureCheck.Models;

namespace TemperatureCheck.Repository
{
    public class StatusDataMapper : IDataMapper
    {
        private readonly SqlConnection _sqlConnection;
        private readonly string _sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public StatusDataMapper()
        {
            _sqlConnection = new SqlConnection(_sqlConnectionString);
        }
        public bool StatusInsert(float tempFahrenheit, float tempCelcius, float humidity)
        {
            try
            {
                if (_sqlConnection.State == ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                // build our SQL Query
                string ourSql = "INSERT INTO [rpi_roomtemp].[Status] ([tempFahrenheit],[tempCelcius],[humidity],[timestamp]) VALUES (@tempCelcius, @tempFahrenheit, @humidity, SYSDATETIME())";

                // init command
                var cmd = new SqlCommand(ourSql)
                {
                    Connection = _sqlConnection
                };

                // tack on parameters
                cmd.Parameters.AddWithValue("@tempFahrenheit", tempFahrenheit);
                cmd.Parameters.AddWithValue("@tempCelcius", tempCelcius);
                cmd.Parameters.AddWithValue("@humidity", humidity);

                // execute SQL
                cmd.ExecuteScalar();

                // close connection
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

        public List<Status> StatusGet(int count, string sortOrder)
        {
            var ourReturnList = new List<Status>();
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
                                "SELECT TOP " + count + " [tempFahrenheit],[tempCelcius],[humidity],[timestamp] FROM [rpi_roomtemp].[rpi_roomtemp].[Status] ORDER BY StatusID " + sortOrder,
                                _sqlConnection))
                    {
                        //
                        // Invoke ExecuteReader method.
                        //
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var ourStatus = new Status()
                            {
                                TempFahrenheit = (float) reader.GetDouble(0),
                                TempCelcius = (float) reader.GetDouble(1),
                                Humidity = (float) reader.GetDouble(2),
                                TimeStamp = reader.GetDateTime(3)
                            };
                            ourReturnList.Add(ourStatus);
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

        // not used here, deprecated
        public bool ReadingInsert(float ourTemperature)
        {
            throw new NotImplementedException();
        }

        // not used here, deprecated
        public List<SingleTempResult> ReadingGet(int count, string sortOrder)
        {
            throw new NotImplementedException();
        }
    }
}