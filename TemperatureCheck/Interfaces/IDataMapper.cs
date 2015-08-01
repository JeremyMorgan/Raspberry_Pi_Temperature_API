using System.Collections.Generic;
using System.Web.Http;
using TemperatureCheck.Models;

namespace TemperatureCheck.Interfaces
{
    interface IDataMapper
    {
        // inserts a reading
        bool ReadingInsert(float ourTemperature);
        // Gets a set of readings
        List<SingleTempResult> ReadingGet(int count, string sortOrder);
    }
}
