using Aspose.Cells;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using System.Drawing;
using AsposeLicense = Aspose.Cells.License;
using Range = Aspose.Cells.Range;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Aspose.Cells
{
    public static class WorkbookExtensions
    {
        internal static ApplicationSettings AppConfig => ApplicationConfiguration.ApplicationSettings;
        private static string? licensePath => AppConfig.AsposeLicensePath;
        public static byte[] ExportToXlxs(this Workbook workbook)
            => workbook.Export(new OoxmlSaveOptions());
        public static byte[] ExportToXls(this Workbook workbook)
            => workbook.Export(new XlsSaveOptions());
        private static byte[] Export(this Workbook workbook, SaveOptions saveOptions)
        {
            byte[] fileData;
            using (var ms = new MemoryStream())
            {
                workbook.Save(ms, saveOptions);
                fileData = ms.ToArray();
            }
            return fileData;
        }
        public static Workbook ImportToWorkbookSingleSheet(string name, params System.Data.DataTable[] dataTables)
        => ImportToWorkbookSingleSheet(name, 0, 0, dataTables);
        public static Workbook ImportToWorkbookSingleSheet(string name, int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            Workbook workbook = new Workbook().CheckAsposeLicense();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = name;
            worksheet.ImportToWorksheet(firstRow, firstCol, dataTables);
            return workbook;
        }
        public static Workbook ImportToWorkbook(params System.Data.DataTable[] dataTables)
        => ImportToWorkbook(0, 0, dataTables);
        public static Workbook ImportToWorkbook(int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            Workbook workbook = new Workbook().CheckAsposeLicense();
            foreach (var dt in dataTables)
            {
                Worksheet worksheet = dt == dataTables.First() ? workbook.Worksheets[0] : workbook.Worksheets[workbook.Worksheets.Add()];
                worksheet.Name = dt.TableName;
                worksheet.ImportToWorksheet(firstRow, firstCol, dt, true);
            }
            return workbook;
        }
        public static Workbook CheckAsposeLicense(this Workbook workbook)
        {
            if (!workbook.IsLicensed && !string.IsNullOrWhiteSpace(licensePath))
            {
                AsposeLicense lic = new AsposeLicense();
                lic.SetLicense(licensePath);
            }
            return workbook;
        }
        public static Workbook ProtectWorkbook(this Workbook workbook, ProtectionType protectionType = ProtectionType.All)
        {
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                worksheet.Protect(protectionType);
            }
            return workbook;
        }
        private static void ImportToWorksheet(this Worksheet worksheet, int firstRow, int firstCol, params System.Data.DataTable[] dataTables)
        {
            int nextRow = firstRow;
            foreach (var dt in dataTables)
            {
                nextRow = worksheet.ImportToWorksheet(nextRow, firstCol, dt, dt == dataTables.Last()) + 1;
            }
        }
        private static void ApplyHeaderStyle(this Range range)
        {
            Style stl = range.Worksheet.Workbook.CreateStyle();
            stl.Font.IsBold = true;
            //Set the font text color
            stl.Font.Color = Color.White;
            stl.ForegroundColor = Color.CadetBlue; //bg color
            stl.Pattern = BackgroundType.Solid;
            //Create a StyleFlag object.
            StyleFlag flg = new StyleFlag();
            //Make the corresponding attributes ON.
            flg.Font = true;
            flg.CellShading = true;
            range.ApplyStyle(stl, flg);
        }
        private static int ImportToWorksheet(this Worksheet worksheet, int firstRow, int firstCol, System.Data.DataTable dataTable, bool autoFilter = false)
        {
            worksheet.Cells.ImportData(dataTable, firstRow, firstCol, new ImportTableOptions() { IsFieldNameShown = true });
            worksheet.AutoFitColumns();
            var tableRange = worksheet.Cells.CreateRange(firstRow, firstCol, dataTable.Rows.Count + 1, dataTable.Columns.Count);
            var headerRange = worksheet.Cells.CreateRange(firstRow, firstCol, 1, dataTable.Columns.Count);

            headerRange.ApplyHeaderStyle();
            headerRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Medium, Color.Black);

            tableRange.SetOutlineBorder(BorderType.TopBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.LeftBorder, CellBorderType.Thick, Color.Black);
            tableRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thick, Color.Black);



            for (int col = firstCol; col < firstCol + dataTable.Columns.Count - 1; col++)
            {
                var colRange = worksheet.Cells.CreateRange(firstRow, col, 1, 1);
                colRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Gray);
            }

            for (int row = firstRow + 1; row < firstRow + dataTable.Rows.Count; row++)
            {
                var rowRange = worksheet.Cells.CreateRange(row, firstCol, 1, dataTable.Columns.Count);
                rowRange.SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Gray);
            }
            for (int col = firstCol; col < firstCol + dataTable.Columns.Count - 1; col++)
            {
                var colRange = worksheet.Cells.CreateRange(firstRow + 1, col, dataTable.Rows.Count, 1);
                colRange.SetOutlineBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Gray);
            }
            if (autoFilter)
            {
                // Creating AutoFilter by giving the cells range of the heading row
                worksheet.AutoFilter.Range = $"{worksheet.Cells[firstRow, firstCol].Name}:{worksheet.Cells[firstRow, firstCol + dataTable.Columns.Count - 1].Name}";
            }

            // Add FormatConditions to the instance of Worksheet
            int idx = worksheet.ConditionalFormattings.Add();

            // Access the newly added FormatConditions via its index
            var conditionCollection = worksheet.ConditionalFormattings[idx];

            // Define a CellsArea on which conditional formatting will be applicable
            // The code creates a CellArea ranging from A1 to I20
            var area = CellArea.CreateCellArea(worksheet.Cells[firstRow + 1, firstCol].Name, worksheet.Cells[firstRow + dataTable.Rows.Count, firstCol + dataTable.Columns.Count - 1].Name);

            //Add area to the instance of FormatConditions
            conditionCollection.AddArea(area);

            // Add a condition to the instance of FormatConditions
            // For this case, the condition type is expression, which is based on some formula
            idx = conditionCollection.AddCondition(FormatConditionType.Expression);

            // Access the newly added FormatCondition via its index
            FormatCondition formatCondirion = conditionCollection[idx];

            // Set the formula for the FormatCondition
            // Formula uses the Excel's built-in functions as discussed earlier in this article
            formatCondirion.Formula1 = @"=MOD(ROW(),2)=0";

            // Set the background color and patter for the FormatCondition's Style
            formatCondirion.Style.BackgroundColor = Color.LightGray;
            formatCondirion.Style.Pattern = BackgroundType.Solid;


            return firstRow + dataTable.Rows.Count;
        }
    }
}