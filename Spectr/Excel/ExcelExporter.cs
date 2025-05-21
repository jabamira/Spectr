using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Spectr.Data;
using Spectr.Db;
using System;
using System.IO;
using System.Linq;

public class ExcelExporter
{
    public ExcelExporter()
    {
    }

    public void ExportAllToExcel(string filePath)
    {
        using var workbook = new XLWorkbook();

        AddSheet(workbook, "Administrators", Db_Helper.context.Administrators);
        AddSheet(workbook, "Customers", Db_Helper.context.Customers);
        AddSheet(workbook, "Contracts", Db_Helper.context.Contracts);
        AddSheet(workbook, "Areas", Db_Helper.context.Areas);
        AddSheet(workbook, "AreaCoordinates", Db_Helper.context.AreaCoordinates);
        AddSheet(workbook, "Profiles", Db_Helper.context.Profiles);
        AddSheet(workbook, "ProfileCoordinates", Db_Helper.context.ProfileCoordinates);
        AddSheet(workbook, "Operators", Db_Helper.context.Operators);
        AddSheet(workbook, "GammaSpectrometers", Db_Helper.context.GammaSpectrometers);
        AddSheet(workbook, "ProfileOperator", Db_Helper.context.ProfileOperator);
        AddSheet(workbook, "ContractAnalyst", Db_Helper.context.ContractAnalyst);
        AddSheet(workbook, "Pickets", Db_Helper.context.Pickets);
        AddSheet(workbook, "Analysts", Db_Helper.context.Analysts);

        workbook.SaveAs(filePath);
    }

    private void AddSheet<T>(XLWorkbook workbook, string sheetName, DbSet<T> dataSet) where T : class
    {
        var data = dataSet.AsNoTracking().ToList();
        var worksheet = workbook.Worksheets.Add(sheetName);

        if (!data.Any()) return;

        var properties = typeof(T).GetProperties()
            .Where(p =>
                p.PropertyType.IsPrimitive
                || p.PropertyType == typeof(string)
                || p.PropertyType == typeof(DateTime)
                || Nullable.GetUnderlyingType(p.PropertyType)?.IsPrimitive == true
                || Nullable.GetUnderlyingType(p.PropertyType) == typeof(DateTime)
            )
            .ToArray();

        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = properties[i].Name;
        }

        for (int row = 0; row < data.Count; row++)
        {
            for (int col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(data[row]);
                worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
            }
        }

        worksheet.Columns().AdjustToContents();
    }



}
