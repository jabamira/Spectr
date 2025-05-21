using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Spectr.Data;
using Spectr.Db;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Spectr.Excel
{
    public class ExcelImporter
    {

        public async Task ImportAllAsync(string filePath)
        {
            try
            {
                using var workbook = new XLWorkbook(filePath);

                await ImportWithSaveAsync(() => ImportAdministratorsAsync(workbook.Worksheet("Administrators")), "Administrators");
                await ImportWithSaveAsync(() => ImportCustomersAsync(workbook.Worksheet("Customers")), "Customers");
                await ImportWithSaveAsync(() => ImportContractsAsync(workbook.Worksheet("Contracts")), "Contracts");
                await ImportWithSaveAsync(() => ImportOperatorsAsync(workbook.Worksheet("Operators")), "Operators");
                await ImportWithSaveAsync(() => ImportProfileOperatorsAsync(workbook.Worksheet("ProfileOperator")), "ProfileOperator");
                await ImportWithSaveAsync(() => ImportAreasAsync(workbook.Worksheet("Areas")), "Areas");
                await ImportWithSaveAsync(() => ImportAreaCoordinatesAsync(workbook.Worksheet("AreaCoordinates")), "AreaCoordinates");

                await ImportWithSaveAsync(() => ImportProfilesAsync(workbook.Worksheet("Profiles")), "Profiles");
                await ImportWithSaveAsync(() => ImportProfileCoordinatesAsync(workbook.Worksheet("ProfileCoordinates")), "ProfileCoordinates");

              
        

                await ImportWithSaveAsync(() => ImportGammaSpectrometersAsync(workbook.Worksheet("GammaSpectrometers")), "GammaSpectrometers");
                await ImportWithSaveAsync(() => ImportPicketsAsync(workbook.Worksheet("Pickets")), "Pickets");

                await ImportWithSaveAsync(() => ImportAnalystsAsync(workbook.Worksheet("Analysts")), "Analysts");
                await ImportWithSaveAsync(() => ImportContractAnalystsAsync(workbook.Worksheet("ContractAnalyst")), "ContractAnalyst");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex);
            }
        }



        private async Task ImportWithSaveAsync(Func<Task> importMethod, string sheetName)
        {
            try
            {
                await importMethod();
                await Db_Helper.context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? "нет вложенного исключения";
                MessageBox.Show($"Ошибка при импорте листа \"{sheetName}\": {ex.Message}\n\nВнутренняя ошибка: {inner}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте листа \"{sheetName}\": {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }



        private int SafeGetInt(IXLCell cell)
        {
            return int.TryParse(cell.GetValue<string>(), out int result) ? result : 0;
        }


        private float? SafeGetNullableFloat(IXLCell cell)
        {
            return float.TryParse(cell.GetValue<string>(), out float result) ? result : (float?)null;
        }

        private float SafeGetFloat(IXLCell cell)
        {
            return float.TryParse(cell.GetValue<string>(), out float result) ? result : 0f;
        }

        private DateTime? SafeGetDateTime(IXLCell cell)
        {
            return DateTime.TryParse(cell.GetValue<string>(), out DateTime result) ? result : (DateTime?)null;
        }

        private string SafeGetString(IXLCell cell)
        {
            var val = cell.GetValue<string>();
            return string.IsNullOrWhiteSpace(val) ? null : val;
        }

        private DateTime? SafeGetNullableDateTime(IXLCell cell)
        {
            return DateTime.TryParse(cell.GetValue<string>(), out DateTime val) ? val : (DateTime?)null;
        }

        private Dictionary<int, int> customerIdMap = new();
        private Dictionary<int, int> contractIdMap = new();
        private Dictionary<int, int> areaIdMap = new();
        private Dictionary<int, int> profileIdMap = new();
        private Dictionary<int, int> operatorIdMap = new();
        private Dictionary<int, int> analystIdMap = new();
        private Dictionary<int, int> gammaSpectrometerIdMap = new();
        private Dictionary<int, int> picketIdMap = new();
        public Dictionary<int, int> administratorIdMap = new();
        public async Task ImportAdministratorsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1); // пропускаем заголовок

            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                string fullName = SafeGetString(row.Cell(2));
                string phoneNumber = SafeGetString(row.Cell(3));
                string email = SafeGetString(row.Cell(4));
                string jobTitle = SafeGetString(row.Cell(5));
                string login = SafeGetString(row.Cell(6));
                string password = SafeGetString(row.Cell(7));

                var existing = Db_Helper.context.Administrators.Local.FirstOrDefault(a => a.AdministratorID == oldId)
                    ?? await Db_Helper.context.Administrators.FindAsync(oldId);

                if (existing == null)
                {
                    var admin = new Administrator
                    {
                        FullName = fullName,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        JobTitle = jobTitle,
                        AdministratorLogin = login,
                        AdministratorPassword = password
                    };

                    Db_Helper.context.Administrators.Add(admin);
                    await Db_Helper.context.SaveChangesAsync();
                    administratorIdMap[oldId] = admin.AdministratorID;
                }
                else
                {
                    existing.FullName = fullName;
                    existing.PhoneNumber = phoneNumber;
                    existing.Email = email;
                    existing.JobTitle = jobTitle;
                    existing.AdministratorLogin = login;
                    existing.AdministratorPassword = password;

                    await Db_Helper.context.SaveChangesAsync();
                    administratorIdMap[oldId] = existing.AdministratorID;
                }
            }
        }
        // Импорт Customers
        public async Task ImportCustomersAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                var existing = Db_Helper.context.Customers.Local.FirstOrDefault(c => c.CustomerID == oldId)
                    ?? Db_Helper.context.Customers.Find(oldId);
                if (existing == null)
                {
                    var customer = new Customer
                    {
                        CompanyName = SafeGetString(row.Cell(2)),
                        PhoneNumber = SafeGetString(row.Cell(3)),
                        ContactPerson = SafeGetString(row.Cell(4)),
                        Email = SafeGetString(row.Cell(5)),
                        Address = SafeGetString(row.Cell(6)),
                        CustomerLogin = SafeGetString(row.Cell(7)),
                        CustomerPassword = SafeGetString(row.Cell(8))
                    };
                    Db_Helper.context.Customers.Add(customer);
                  await   Db_Helper.context.SaveChangesAsync();
                    customerIdMap[oldId] = customer.CustomerID;
                }
                else
                {
                    existing.CompanyName = SafeGetString(row.Cell(2));
                    existing.PhoneNumber = SafeGetString(row.Cell(3));
                    existing.ContactPerson = SafeGetString(row.Cell(4));
                    existing.Email = SafeGetString(row.Cell(5));
                    existing.Address = SafeGetString(row.Cell(6));
                    existing.CustomerLogin = SafeGetString(row.Cell(7));
                    existing.CustomerPassword = SafeGetString(row.Cell(8));
                  await   Db_Helper.context.SaveChangesAsync();
                    customerIdMap[oldId] = existing.CustomerID;
                }
            }
        }

        public async Task ImportContractsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                int oldCustomerId = SafeGetInt(row.Cell(2));

                var existing = Db_Helper.context.Contracts.Local.FirstOrDefault(c => c.ContractID == oldId)
                    ?? Db_Helper.context.Contracts.Find(oldId);

                int? newCustomerId = null;
                if (customerIdMap.TryGetValue(oldCustomerId, out int mappedCustomerId))
                    newCustomerId = mappedCustomerId;

                if (existing == null)
                {
                    var contract = new Contract
                    {
                        StartDate = SafeGetDateTime(row.Cell(3)) ?? DateTime.MinValue,
                        EndDate = SafeGetDateTime(row.Cell(4)) ?? DateTime.MinValue,
                        ServiceDescription = SafeGetString(row.Cell(5)),
                        CustomerID = newCustomerId,
                        AdministratorID = SafeGetInt(row.Cell(6))
                    };
                    Db_Helper.context.Contracts.Add(contract);
                  await   Db_Helper.context.SaveChangesAsync();
                    contractIdMap[oldId] = contract.ContractID;
                }
                else
                {
                    existing.StartDate = SafeGetDateTime(row.Cell(3)) ?? DateTime.MinValue;
                    existing.EndDate = SafeGetDateTime(row.Cell(4)) ?? DateTime.MinValue;
                    existing.ServiceDescription = SafeGetString(row.Cell(5));
                    existing.CustomerID = newCustomerId;
                    existing.AdministratorID = SafeGetInt(row.Cell(6));
                  await   Db_Helper.context.SaveChangesAsync();
                    contractIdMap[oldId] = existing.ContractID;
                }
            }
        }

        public async Task ImportAreasAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                var existing = Db_Helper.context.Areas.Local.FirstOrDefault(a => a.AreaID == oldId)
                    ?? Db_Helper.context.Areas.Find(oldId);

                if (existing == null)
                {
                    var area = new Area
                    {
                        AreaName = SafeGetString(row.Cell(2))
                    };
                    Db_Helper.context.Areas.Add(area);
                  await   Db_Helper.context.SaveChangesAsync();
                    areaIdMap[oldId] = area.AreaID;
                }
                else
                {
                    existing.AreaName = SafeGetString(row.Cell(2));
                  await   Db_Helper.context.SaveChangesAsync();
                    areaIdMap[oldId] = existing.AreaID;
                }
            }
        }

        public async Task ImportAreaCoordinatesAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldAreaId = SafeGetInt(row.Cell(4));
                if (!areaIdMap.TryGetValue(oldAreaId, out int newAreaId))
                    continue; // или кинуть исключение

                var coord = new AreaCoordinates
                {
                    X = SafeGetFloat(row.Cell(2)),
                    Y = SafeGetFloat(row.Cell(3)),
                    AreaID = newAreaId
                };
                Db_Helper.context.AreaCoordinates.Add(coord);
            }
          await   Db_Helper.context.SaveChangesAsync();
        }

        public async Task ImportProfilesAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                int oldAreaId = SafeGetInt(row.Cell(4));
                if (!areaIdMap.TryGetValue(oldAreaId, out int newAreaId))
                    continue;

                var existing = Db_Helper.context.Profiles.Local.FirstOrDefault(p => p.ProfileID == oldId)
                    ?? Db_Helper.context.Profiles.Find(oldId);

                if (existing == null)
                {
                    var profile = new Profile
                    {
                        ProfileName = SafeGetString(row.Cell(2)),
                        ProfileType = SafeGetString(row.Cell(3)),
                        AreaID = newAreaId
                    };
                    Db_Helper.context.Profiles.Add(profile);
                  await   Db_Helper.context.SaveChangesAsync();
                    profileIdMap[oldId] = profile.ProfileID;
                }
                else
                {
                    existing.ProfileName = SafeGetString(row.Cell(2));
                    existing.ProfileType = SafeGetString(row.Cell(3));
                    existing.AreaID = newAreaId;
                  await   Db_Helper.context.SaveChangesAsync();
                    profileIdMap[oldId] = existing.ProfileID;
                }
            }
        }

        public async Task ImportProfileCoordinatesAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldProfileId = SafeGetInt(row.Cell(4));
                if (!profileIdMap.TryGetValue(oldProfileId, out int newProfileId))
                    continue;

                var coord = new ProfileCoordinates
                {
                    X = SafeGetFloat(row.Cell(2)),
                    Y = SafeGetFloat(row.Cell(3)),
                    ProfileID = newProfileId
                };
                Db_Helper.context.ProfileCoordinates.Add(coord);
            }
          await   Db_Helper.context.SaveChangesAsync();
        }

        public async Task ImportOperatorsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                var existing = Db_Helper.context.Operators.Local.FirstOrDefault(o => o.OperatorID == oldId)
                    ?? Db_Helper.context.Operators.Find(oldId);

                if (existing == null)
                {
                    var oper = new Operator
                    {
                        FullName = SafeGetString(row.Cell(2)),
                        PhoneNumber = SafeGetString(row.Cell(3)),
                        Email = SafeGetString(row.Cell(4)),
                        JobTitle = SafeGetString(row.Cell(5)),
                        OperatorLogin = SafeGetString(row.Cell(6)),
                        OperatorPassword = SafeGetString(row.Cell(7))
                    };
                    Db_Helper.context.Operators.Add(oper);
                  await   Db_Helper.context.SaveChangesAsync();
                    operatorIdMap[oldId] = oper.OperatorID;
                }
                else
                {
                    existing.FullName = SafeGetString(row.Cell(2));
                    existing.PhoneNumber = SafeGetString(row.Cell(3));
                    existing.Email = SafeGetString(row.Cell(4));
                    existing.JobTitle = SafeGetString(row.Cell(5));
                    existing.OperatorLogin = SafeGetString(row.Cell(6));
                    existing.OperatorPassword = SafeGetString(row.Cell(7));
                  await   Db_Helper.context.SaveChangesAsync();
                    operatorIdMap[oldId] = existing.OperatorID;
                }
            }
        }

        public async Task ImportAnalystsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                var existing = Db_Helper.context.Analysts.Local.FirstOrDefault(a => a.AnalystID == oldId)
                    ?? Db_Helper.context.Analysts.Find(oldId);

                if (existing == null)
                {
                    var analyst = new Analyst
                    {
                        FullName = SafeGetString(row.Cell(2)),
                        PhoneNumber = SafeGetString(row.Cell(3)),
                        Email = SafeGetString(row.Cell(4)),
                        JobTitle = SafeGetString(row.Cell(5)),
                        AnalystLogin = SafeGetString(row.Cell(6)),
                        AnalystPassword = SafeGetString(row.Cell(7))
                    };
                    Db_Helper.context.Analysts.Add(analyst);
                  await   Db_Helper.context.SaveChangesAsync();
                    analystIdMap[oldId] = analyst.AnalystID;
                }
                else
                {
                    existing.FullName = SafeGetString(row.Cell(2));
                    existing.PhoneNumber = SafeGetString(row.Cell(3));
                    existing.Email = SafeGetString(row.Cell(4));
                    existing.JobTitle = SafeGetString(row.Cell(5));
                    existing.AnalystLogin = SafeGetString(row.Cell(6));
                    existing.AnalystPassword = SafeGetString(row.Cell(7));
                  await   Db_Helper.context.SaveChangesAsync();
                    analystIdMap[oldId] = existing.AnalystID;
                }
            }
        }

        // Импорт ContractAnalysts
        public async Task ImportContractAnalystsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldContractId = SafeGetInt(row.Cell(1));
                int oldAnalystId = SafeGetInt(row.Cell(2));

                if (!contractIdMap.TryGetValue(oldContractId, out int newContractId)) continue;
                if (!analystIdMap.TryGetValue(oldAnalystId, out int newAnalystId)) continue;

                bool exists = Db_Helper.context.ContractAnalyst.Any(ca => ca.ContractID == newContractId && ca.AnalystID == newAnalystId);
                if (!exists)
                {
                    var contractAnalyst = new ContractAnalyst
                    {
                        ContractID = newContractId,
                        AnalystID = newAnalystId
                    };
                    Db_Helper.context.ContractAnalyst.Add(contractAnalyst);
                }
            }
          await   Db_Helper.context.SaveChangesAsync();
        }
        public async Task ImportGammaSpectrometersAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                var commissioningDate = SafeGetNullableDateTime(row.Cell(2));
                if (commissioningDate == null)
                    throw new Exception("CommissioningDate не может быть пустым"); var decommissioningDate = SafeGetNullableDateTime(row.Cell(3));
                var existing = Db_Helper.context.GammaSpectrometers.Local.FirstOrDefault(g => g.GammaSpectrometerID == oldId)
                    ?? Db_Helper.context.GammaSpectrometers.Find(oldId);

                if (existing == null)
                {
                    var spectrometer = new GammaSpectrometer
                    {
                        CommissioningDate = commissioningDate.Value,
                        DecommissioningDate = decommissioningDate,
                        MeasurementAccuracy = SafeGetFloat(row.Cell(4)),
                        MeasurementTime = SafeGetInt(row.Cell(5))
                    };
                    Db_Helper.context.GammaSpectrometers.Add(spectrometer);
                  await   Db_Helper.context.SaveChangesAsync();
                    gammaSpectrometerIdMap[oldId] = spectrometer.GammaSpectrometerID;
                }
                else
                {
                    existing.CommissioningDate = commissioningDate.Value;
                    existing.DecommissioningDate = decommissioningDate;
                    existing.MeasurementAccuracy = SafeGetFloat(row.Cell(4));
                    existing.MeasurementTime = SafeGetInt(row.Cell(5));
                  await   Db_Helper.context.SaveChangesAsync();
                    gammaSpectrometerIdMap[oldId] = existing.GammaSpectrometerID;
                }
            }
        }

        // Импорт ProfileOperators
        public async Task ImportProfileOperatorsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                int profileOldId = SafeGetInt(row.Cell(1));
                int operatorOldId = SafeGetInt(row.Cell(2));

                int profileNewId = profileIdMap[profileOldId];
                int operatorNewId = operatorIdMap[operatorOldId];

                bool exists = Db_Helper.context.ProfileOperator.Any(po => po.ProfileID == profileNewId && po.OperatorID == operatorNewId);
                if (!exists)
                {
                    var profileOperator = new ProfileOperator
                    {
                        ProfileID = profileNewId,
                        OperatorID = operatorNewId
                    };
                    Db_Helper.context.ProfileOperator.Add(profileOperator);
                }
            }
          await   Db_Helper.context.SaveChangesAsync();
        }

        public async Task  ImportPicketsAsync(IXLWorksheet worksheet)
        {
            var rows = worksheet.RowsUsed().Skip(1); // Пропускаем заголовок

            foreach (var row in rows)
            {
                int oldId = SafeGetInt(row.Cell(1));
                float coordinateX = SafeGetFloat(row.Cell(2));
                float coordinateY = SafeGetFloat(row.Cell(3));
                float? channel1 = SafeGetNullableFloat(row.Cell(4));
                float? channel2 = SafeGetNullableFloat(row.Cell(5));
                float? channel3 = SafeGetNullableFloat(row.Cell(6));
                int oldProfileId = SafeGetInt(row.Cell(7));
                int oldOperatorId = SafeGetInt(row.Cell(8));
                int oldSpectrometerId = SafeGetInt(row.Cell(9));

                // Проверка наличия всех необходимых внешних ключей
                if (!profileIdMap.TryGetValue(oldProfileId, out int newProfileId)) continue;
                if (!operatorIdMap.TryGetValue(oldOperatorId, out int newOperatorId)) continue;
                if (!gammaSpectrometerIdMap.TryGetValue(oldSpectrometerId, out int newSpectrometerId)) continue;

                // Проверка на существующий пикет (можно адаптировать под свои уникальные признаки)
                var existing = Db_Helper.context.Pickets
                    .FirstOrDefault(p => p.CoordinateX == coordinateX &&
                                         p.CoordinateY == coordinateY &&
                                         p.ProfileID == newProfileId);

                if (existing != null)
                {
                    // Обновление существующего
                    existing.Channel1 = channel1;
                    existing.Channel2 = channel2;
                    existing.Channel3 = channel3;
                    existing.OperatorID = newOperatorId;
                    existing.SpectrometerID = newSpectrometerId;

                    Db_Helper.context.Pickets.Update(existing);
                  await   Db_Helper.context.SaveChangesAsync();

                    picketIdMap[oldId] = existing.PicketID;
                }
                else
                {
                    // Добавление нового
                    var picket = new Picket
                    {
                        CoordinateX = coordinateX,
                        CoordinateY = coordinateY,
                        Channel1 = channel1,
                        Channel2 = channel2,
                        Channel3 = channel3,
                        ProfileID = newProfileId,
                        OperatorID = newOperatorId,
                        SpectrometerID = newSpectrometerId
                    };

                    Db_Helper.context.Pickets.Add(picket);
                  await   Db_Helper.context.SaveChangesAsync();

                    picketIdMap[oldId] = picket.PicketID;
                }
            }
        }


    }
}