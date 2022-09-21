using System.Data;

namespace NET6CoreWebAppWithbootstrap5.Extensions
{
    public static class SystemDataTableExtensions
    {
        public static DataTable ToSystemDataTable<T>(this IEnumerable<T> data, params (string? name, Func<T, object?> getValue)[] columns)
            => data.ToSystemDataTable("DataTable", columns);
        public static DataTable ToSystemDataTable<T>(this IEnumerable<T> data, string name, params (string? name, Func<T, object?> getValue)[] columns)
        {
            DataTable dataTable = new DataTable(name);
            foreach (var column in columns)
            {
                dataTable.Columns.Add(column.name, typeof(object));
            }
            foreach (T model in data)
            {
                DataRow dr = dataTable.NewRow();
                int rowIndex = 0;
                foreach (var column in columns)
                {
                    dr[rowIndex] = column.getValue(model);
                    rowIndex++;
                }
                dataTable.Rows.Add(dr);
            }
            return dataTable;
        }
    }
}
