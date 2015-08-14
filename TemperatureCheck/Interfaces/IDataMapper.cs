using System.Collections.Generic;
using System.Web.Http;
using TemperatureCheck.Models;

namespace TemperatureCheck.Interfaces
{
    interface IDataMapper
    {
        // inserts a reading (Deprecated)
        bool ReadingInsert(float ourTemperature);

        bool StatusInsert(float tempFahrenheit, float tempCelcius, float humidity);

        // Gets a set of readings (Deprecated)
        List<SingleTempResult> ReadingGet(int count, string sortOrder);

        List<Status> StatusGet(int count, string sortOrder);
    }
}
