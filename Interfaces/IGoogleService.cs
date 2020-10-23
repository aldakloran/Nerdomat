using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Nerdomat.Models;

namespace Nerdomat.Interfaces
{
    public interface IGoogleService
    {
        Lazy<SheetsService> SheetsService { get; }

        Task<UpdateValuesResponse> WriteDataAsync(string rangeData, ValueRange valueRange);
        Task<UpdateValuesResponse> WriteDataAsync(string rangeData, object data);
        Task<UpdateValuesResponse> WriteDataAsync(string rangeData, IEnumerable<object> data);
        Task<List<List<string>>> ReadDataAsync(string rangeData);
        Task<string> ReadCellAsync(string rangeData);
        Task<List<T>> ReadDataAsync<T>(string rangeData) where T : class;
        Task<List<NerdModel>> GetNerdsAsync();
    }
}