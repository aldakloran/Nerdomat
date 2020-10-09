using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Nerdomat.Tools
{
    public static class GoogleSheetsExtensions
    {
        public static Task<UpdateValuesResponse> WriteDataAsync(this SheetsService service, string spreadsheet, string rangeData, ValueRange valueRange) {
            return Task.Factory.StartNew(() => {
                var update = service.Spreadsheets.Values.Update(valueRange, spreadsheet, rangeData);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                return update.Execute();
            });
        }

        public static Task<List<List<string>>> ReadDataAsync(this SheetsService service, string spreadsheet, string rangeData) {
            return Task.Factory.StartNew(() => {
                var request  = service.Spreadsheets.Values.Get(spreadsheet, rangeData);
                var response = request.Execute();
                var values   = response.Values;

                var returnList = new List<List<string>>();
                foreach (var row in values) {
                    var o = new List<string>();
                    o.AddRange(row.Select(x => x.ToString()));
                    returnList.Add(o);
                }

                return returnList;
            });
        }

        public static Task<List<T>> ReadDataAsync<T>(this SheetsService service, string spreadsheet, string rangeData) where T : class {
            return Task.Factory.StartNew(() => {
                var request  = service.Spreadsheets.Values.Get(spreadsheet, rangeData);
                var response = request.Execute();
                var values   = response.Values;

                var returnList = new List<T>();
                foreach (var row in values) {
                    var newObject = Activator.CreateInstance(typeof(T), true) as T;

                    var i = 0;
                    foreach (var field in typeof(T).GetProperties().OrderBy(x => x.MetadataToken)) {
                        if (i >= row.Count) break;

                        var val = row[i++].ToString();

                        var typ = Type.GetTypeCode(field.PropertyType);
                        if (field.PropertyType.IsGenericType && field.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                            typ = Type.GetTypeCode(field.PropertyType.GetGenericArguments()[0]);
                        }

                        switch (typ) {
                            case TypeCode.String:
                                field.SetValue(newObject, val);

                                break;
                            case TypeCode.Boolean:
                                if (bool.TryParse(val, out var res1))
                                    field.SetValue(newObject, res1);

                                break;
                            case TypeCode.Byte:
                                if (byte.TryParse(val, out var res2))
                                    field.SetValue(newObject, res2);

                                break;
                            case TypeCode.Char:
                                if (char.TryParse(val, out var res3))
                                    field.SetValue(newObject, res3);

                                break;
                            case TypeCode.DateTime:
                                if (DateTime.TryParse(val, out var res4))
                                    field.SetValue(newObject, res4);

                                break;
                            case TypeCode.Decimal:
                                if (decimal.TryParse(val, out var res5))
                                    field.SetValue(newObject, res5);

                                break;
                            case TypeCode.Double:
                                if (double.TryParse(val, out var res6))
                                    field.SetValue(newObject, res6);

                                break;
                            case TypeCode.Int16:
                                if (short.TryParse(val, out var res7))
                                    field.SetValue(newObject, res7);

                                break;
                            case TypeCode.Int32:
                                if (int.TryParse(val, out var res8))
                                    field.SetValue(newObject, res8);

                                break;
                            case TypeCode.Int64:
                                if (long.TryParse(val, out var res9))
                                    field.SetValue(newObject, res9);

                                break;
                            case TypeCode.SByte:
                                if (sbyte.TryParse(val, out var res10))
                                    field.SetValue(newObject, res10);

                                break;
                            case TypeCode.Single:
                                if (float.TryParse(val, out var res11))
                                    field.SetValue(newObject, res11);

                                break;
                            case TypeCode.UInt16:
                                if (ushort.TryParse(val, out var res12))
                                    field.SetValue(newObject, res12);

                                break;
                            case TypeCode.UInt32:
                                if (uint.TryParse(val, out var res13))
                                    field.SetValue(newObject, res13);

                                break;
                            case TypeCode.UInt64:
                                if (ulong.TryParse(val, out var res14))
                                    field.SetValue(newObject, res14);

                                break;
                            default:
                                Debug.WriteLine(@"Inny typ");
                                try {
                                    object newVal = Convert.ChangeType(val, field.PropertyType);
                                    field.SetValue(newObject, newVal);
                                }
                                catch {
                                    // ignored
                                }

                                break;
                        }
                    }

                    returnList.Add(newObject);
                }

                return returnList;
            });
        }
    }
    
}