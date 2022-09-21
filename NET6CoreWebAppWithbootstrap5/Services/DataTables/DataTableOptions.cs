using NET6CoreWebAppWithbootstrap5.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NET6CoreWebAppWithbootstrap5.Services.DataTables
{
    public class DataTableViewVM
    {
        public string? OptionsJson { get; set; }
        public string? OptionsUrl { get; set; }
        public string? Form { get; set; }
        public bool AppendForm { get; set; }
        public string? Id { get; set; }
    }
    public class DataTableViewVM<T> : DataTableViewVM where T : class
    {
        public T? Model { get; set; }
    }
    public class Parameterless { }
    public class DataTableOptions<T>
        where T : class
    {
        [JsonProperty("serverSide")]
        [JsonPropertyName("serverSide")]
        public bool ServerSide { get; set; }

        [JsonProperty("searchDelay")]
        [JsonPropertyName("searchDelay")]
        public int SearchDelay { get; set; }

        [JsonProperty("processing")]
        [JsonPropertyName("processing")]
        public bool Processing { get; set; }

        [JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("buttons")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] Buttons { get; set; } = new string[0];


        [JsonProperty("ajax")]
        [JsonPropertyName("ajax")]
        public DataTableOptionsAjax? Ajax { get; set; }

        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("order")]
        public object[][] Order { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public DataTableColumnOptions<T>[] Columns { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string DataName { get; set; }

        [JsonProperty("columns")]
        [JsonPropertyName("columns")]
        public DataTableColumnOptions<T>[] DisplayedColumns => Columns.Where(c => !c.DataOnly).ToArray();

        [JsonProperty("lengthChange")]
        [JsonPropertyName("lengthChange")]
        public bool LengthChange { get; set; }

        [JsonProperty("lengthMenu")]
        [JsonPropertyName("lengthMenu")]
        public object[] LengthMenu { get; set; }

        [JsonProperty("pageLength")]
        [JsonPropertyName("pageLength")]
        public int PageLength { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public PagingType PagingType { get; set; }

        [JsonProperty("pagingType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("pagingType")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PagingTypeValue => PagingType.GetPagingType();

        [JsonProperty("scrollX")]
        [JsonPropertyName("scrollX")]
        public bool ScrollX { get; set; }
        [JsonProperty("scrollY", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("scrollY")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ScrollY { get; set; }
        [JsonProperty("search", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("search")]
        public DataTableOptionsSearch Search { get; set; }
        [JsonProperty("searchCols", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("searchCols")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataTableOptionsSearch[] SearchCols { get; set; }

        [JsonProperty("colReorder", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("colReorder")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DataTableOptionsColReorder? ColReorder { get; set; }

        [JsonProperty("download")]
        [JsonPropertyName("download")]
        public DataTableOptionsDownload Download { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<T, string>? GetRowId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<T, string>? GetRowClass { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<T, object>? GetRowData { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<T, object>? GetRowAttr { get; set; }


        public DataTableOptions(params DataTableColumnOptions<T>[] columns)
        {
            Columns = columns;
            ServerSide = true;
            Processing = true;
            SearchDelay = 400;
            Buttons = new string[0];
            Order = new object[0][];
            LengthChange = true;
            LengthMenu = new object[] { 10, 25, 50, 100 };
            PageLength = 10;
            ScrollY = null;
            Search = new DataTableOptionsSearch();
            SearchCols = new DataTableOptionsSearch[0];
            ColReorder = null;
            PagingType = PagingType.SimpleNumbers;
            Download = DataTableOptionsDownload.DisabledDownload;
            DataName = "DataTable";
        }
        public static DataTableOptions<T> Create(
            Action<DataTableOptions<T>> optionsConfig,
            Action<DataTableOptionsAjax> ajaxConfig,
            Action<IDataTableColumnBuilder<T>> columnConfig)
        {
            var ajax = new DataTableOptionsAjax();
            ajaxConfig(ajax);
            var columnBuilder = new DataTableColumnBuilder<T>();
            columnConfig(columnBuilder);
            var options = new DataTableOptions<T>(columnBuilder.ToArray()) { Ajax = ajax };
            optionsConfig(options);
            if (!options.Download.Enabled && options.Buttons is string[] buttons && buttons.Contains("download"))
            {
                options.Buttons = buttons.Where(b => b != "download").ToArray();
            }
            return options;
        }
        public static DataTableOptions<T> Create(
            Action<DataTableOptionsAjax> ajaxConfig,
            Action<IDataTableColumnBuilder<T>> columnConfig)
        => Create(o => { }, ajaxConfig, columnConfig);

        public static DataTableColumnOptions<T> CreateColumn(Func<T, object?> getValue) => new DataTableColumnOptions<T>(getValue);
        internal IEnumerable<IDictionary> GetData(
            IEnumerable<T> data)
        {
            HybridDictionary[] dataForTable = new HybridDictionary[data.Count()];
            int index = 0;
            foreach (T model in data)
            {
                HybridDictionary item = new HybridDictionary();
                if (GetRowId != null)
                {
                    item.TryAdd("DT_RowId", GetRowId(model));
                }
                if (GetRowClass != null)
                {
                    item.TryAdd("DT_RowClass", GetRowClass(model));
                }
                if (GetRowData != null)
                {
                    item.TryAdd("DT_RowData", GetRowData(model));
                }
                if (GetRowAttr != null)
                {
                    item.TryAdd("DT_RowAttr", GetRowAttr(model));
                }
                foreach (DataTableColumnOptions<T> column in Columns.Where(c => !string.IsNullOrWhiteSpace(c.Data)))
                {
                    if(column.Data != null)
                    {
                        item.TryAdd(column.Data, column.RenderValue(model));
                    }
                    
                }
                dataForTable[index] = item;
                index++;
            }
            return dataForTable;
        }
        internal DataTable ToSystemDataTable(IEnumerable<T> data, string name)
        {
            (string? name, Func<T, object?> getValue)[] dtColumns = Columns.Where(c => c.CanExport()).Select(c => (c.Title, (Func<T, object?>)(m => c.GetExportValue(m)))).ToArray();
            return data.ToSystemDataTable(name, dtColumns);
        }
    }

    public class DataTableOptionsAjax
    {
        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
        public DataTableOptionsAjax()
        {
            Type = "POST";
        }
    }
    public class DataTableOptionsSearch
    {
        [JsonProperty("search")]
        [JsonPropertyName("search")]
        public string? Search { get; set; }
        [JsonProperty("caseInsensitive")]
        [JsonPropertyName("caseInsensitive")]
        public bool CaseInsensitive { get; set; }
        [JsonProperty("regex")]
        [JsonPropertyName("regex")]
        public bool Regex { get; set; }
        [JsonProperty("smart")]
        [JsonPropertyName("smart")]
        public bool Smart { get; set; }
        public DataTableOptionsSearch()
        {
            CaseInsensitive = true;
            Smart = true;
        }
    }
    public enum PagingType
    {
        Numbers,
        Simple,
        SimpleNumbers,
        Full,
        FullNumbers,
        FirstLastNumbrs
    }
    internal static class DataTableOptionsPaging
    {
        public static string GetPagingType(this PagingType type)
            => type == PagingType.Numbers ? "numbers" :
            type == PagingType.Simple ? "simple" :
            type == PagingType.SimpleNumbers ? "simple_numbers" :
            type == PagingType.Full ? "full" :
            type == PagingType.FullNumbers ? "full_numbers" :
            type == PagingType.FirstLastNumbrs ? "first_last_numbers" : "simple_numbers";

    }
    public interface IDataTableColumnBuilder<T>
        where T : class
    {
        void Add(Action<DataTableColumnOptions<T>> handler);
        void Add(Func<T, object?> getValue, Action<DataTableColumnOptions<T>> handler);
    }
    internal class DataTableColumnBuilder<T> : List<DataTableColumnOptions<T>>, IDataTableColumnBuilder<T>
        where T : class
    {
        public void Add(Action<DataTableColumnOptions<T>> handler)
        {
            var options = new DataTableColumnOptions<T>();
            handler(options);
            Add(options);
        }
        public void Add(Func<T, object?> getValue, Action<DataTableColumnOptions<T>> handler)
        {
            var options = new DataTableColumnOptions<T>(getValue);
            handler(options);
            Add(options);
        }
    }
    public static class DataTableColumnOptionsExtensions
    {
        public static string GetDateStringValue(this DateTime? value, string format = "MM/dd/yyyy") => value is DateTime d ? d.ToString(format) : "";
    }
    public class DataTableColumnFilterHelpers
    {
        public static Func<string, string> GetDateStringValue(string format = "MM/dd/yyyy")
            => value => DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d) ? d.ToString(format) : value;

    }
    public class DataTableColumnOptions<T>
        where T : class
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public string? Data { get; set; }
        [JsonProperty("className")]
        [JsonPropertyName("className")]
        public string? ClassName { get; set; }
        [JsonProperty("searchable")]
        [JsonPropertyName("searchable")]
        public bool Searchable { get; set; }
        [JsonProperty("orderable")]
        [JsonPropertyName("orderable")]
        public bool Orderable { get; set; }
        [JsonProperty("visible")]
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }
        [JsonProperty("filter")]
        [JsonPropertyName("filter")]
        public DataTableOptionsFilter? Filter { get; set; }



        internal string GetFilterValue(string value)
        {
            try
            {
                return Filter != null ? Filter.Value(value) : value;
            }
            catch
            {
                return value;
            }
        }


        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool DataOnly { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool Export { get; set; }
        internal bool CanExport() => !DataOnly && !string.IsNullOrWhiteSpace(Data) && Export;
        internal Func<T, object?>? GetValue { get; set; }
        internal Func<T, object?>? Render { get; set; }
        internal object? RenderValue(T model)
            => Render is Func<T, object?> render ? render(model) : GetValue is Func<T, object?> getValue ? getValue(model): null;

        internal Func<T, object?>? ExportValue { get; set; }
        internal object? GetExportValue(T model)
            => ExportValue is Func<T, object?> exportValue ? exportValue(model) : GetValue is Func<T, object?> getValue ? getValue(model) : null;

        public DataTableColumnOptions()
        {
            Searchable = true;
            Orderable = true;
            Visible = true;
            Data = null;
            GetValue = m => string.IsNullOrWhiteSpace(Data) ? null : GetPropertyValue(m, Data);
            Export = true;
        }
        public DataTableColumnOptions(Func<T, object?> getValue)
        {
            Searchable = true;
            Orderable = true;
            Visible = true;
            Data = null;
            GetValue = getValue;
            Export = true;
        }
        private static object? GetPropertyValue(object? src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
    }
    public class DataTableOptionsFilter
    {

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonProperty("label")]
        [JsonPropertyName("label")]
        public string? Label { get; set; }
        [JsonProperty("options")]
        [JsonPropertyName("options")]
        public object?[][] Options { get; set; } = new object?[0][];

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<string, string> Value { get; set; }
        public DataTableOptionsFilter()
        {
            Type = "text";
            Value = val => val;
        }
        public static DataTableOptionsFilter CreateDateFilter(string format = "MM/dd/yyyy")
        {
            return new DataTableOptionsFilter()
            {
                Type = "date",
                Value = DataTableColumnFilterHelpers.GetDateStringValue(format)
            };
        }
        private static DataTableOptionsFilter CreateWithOptionsFilter(object?[][] options, string type)
        {
            return new DataTableOptionsFilter()
            {
                Type = type,
                Options = options
            };
        }
        public static DataTableOptionsFilter CreateSelectFilter(object?[][] options)
        => CreateWithOptionsFilter(options, "select");
        public static DataTableOptionsFilter CreateSelectFilter<T>(IEnumerable<T> list, Func<T, object?> valueSelector, Func<T, object?> labelSelector)
            => CreateSelectFilter(list.Select(m => new[] { valueSelector(m), labelSelector(m) }).ToArray());
        public static DataTableOptionsFilter CreateSelectFilter<T>(IEnumerable<T> list, Func<T, object?> valueSelector)
        => CreateSelectFilter(list, valueSelector, valueSelector);

        public static DataTableOptionsFilter CreateRadioFilter(object?[][] options)
        => CreateWithOptionsFilter(options, "radio");
        public static DataTableOptionsFilter CreateRadioFilter<T>(IEnumerable<T> list, Func<T, object?> valueSelector, Func<T, object?> labelSelector)
            => CreateRadioFilter(list.Select(m => new[] { valueSelector(m), labelSelector(m) }).ToArray());
        public static DataTableOptionsFilter CreateRadioFilter<T>(IEnumerable<T> list, Func<T, object?> valueSelector)
        => CreateRadioFilter(list, valueSelector, valueSelector);
    }

    public class DataTableOptionsDownload
    {

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonProperty("downloadAll")]
        [JsonPropertyName("downloadAll")]
        public bool DownloadAll { get; set; }
        [JsonProperty("enabled")]
        [JsonPropertyName("enabled")]
        public bool Enabled { get; private set; }
        public DataTableOptionsDownload()
        {
            Url = null;
            DownloadAll = true;
            Enabled = true;
        }
        internal static DataTableOptionsDownload DisabledDownload = new DataTableOptionsDownload() { Enabled = false };
    }

    public class DataTableOptionsColReorder
    {
        [JsonProperty("enable")]
        [JsonPropertyName("enable")]
        public bool Enable { get; set; }

        [JsonProperty("fixedColumnsLeft")]
        [JsonPropertyName("fixedColumnsLeft")]
        public int FixedColumnsLeft { get; set; }

        [JsonProperty("fixedColumnsRight")]
        [JsonPropertyName("fixedColumnsRight")]
        public int FixedColumnsRight { get; set; }

        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("order")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int[] Order { get; set; }
        [JsonProperty("realtime")]
        [JsonPropertyName("realtime")]
        public bool Realtime { get; set; }

        public DataTableOptionsColReorder()
        {
            Enable = true;
            FixedColumnsLeft = 0;
            FixedColumnsRight = 0;
            Order = new int[0];
            Realtime = true;
        }
    }
    public static class DataTableConverter<T>
    {
        private static Type Type = typeof(T);
        private static PropertyInfo[] PropertyInfo = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        private static DataColumn[] DataColumns => PropertyInfo.Select(p => new DataColumn(p.Name)).ToArray();
        private static DataTable CreateTable() => new DataTable(Type.Name);
        public static DataTable ToDataTable(IEnumerable<T> items)
        {
            DataTable dataTable = CreateTable();
            dataTable.Columns.AddRange(DataColumns);
            foreach (T item in items)
            {
                var values = PropertyInfo.Select(p => p.GetValue(item, null));
                dataTable.Rows.Add(values.ToArray());
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
    public static class DataTableConverter<T,U>
    {
        private static Type Type = typeof(T);
        private static Type TypeAdditionalData = typeof(U);
        private static PropertyInfo[] PropertyInfo = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        private static PropertyInfo[] PropertyInfoAdditionalData = TypeAdditionalData.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        private static DataColumn[] DataColumns => PropertyInfo.Select(p => new DataColumn(p.Name)).Concat(PropertyInfoAdditionalData.Select(p => new DataColumn(p.Name))).ToArray();
        private static DataTable CreateTable(string name) => new DataTable(name);
        public static DataTable ToDataTable(string name, IEnumerable<(T row, U additionalRowData)> items)
        {
            DataTable dataTable = CreateTable(name);
            dataTable.Columns.AddRange(DataColumns);
            foreach ((T row, U additionalRowData) item in items)
            {
                var values = PropertyInfo.Select(p => p.GetValue(item.row, null));
                var valuesAdditionalData = PropertyInfoAdditionalData.Select(p => p.GetValue(item.additionalRowData, null));
                dataTable.Rows.Add(values.Concat(valuesAdditionalData).ToArray());
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
    public static class DataTableExtensions
    {
        private static DataView Sort(this DataView dv, string sortQuery)
        {
            if (!string.IsNullOrWhiteSpace(sortQuery))
            {
                dv.Sort = sortQuery;
            }
            return dv;
        }
        private static DataView Filter(this DataView dv, string filterQuery)
        {
            if (!string.IsNullOrWhiteSpace(filterQuery))
            {
                dv.RowFilter = filterQuery;
            }
            return dv;
        }
        private static DataTable Process(this DataTable dt, string filterQuery, string sortQuery, int startRow, int numberOfRows)
        {
            DataView dv = new DataView(dt).Filter(filterQuery).Sort(sortQuery);
            var rows = dv.ToTable().AsEnumerable().Skip(startRow - 1).Take(numberOfRows);
            return rows.CopyToDataTable();
        }
        private static string ToSortQuery(this object[] order)
            => order.Count() > 1 ? $"{order[0]} {order[1]}" : $"{order[0]} ASC";
        private static string ToSortQuery(this object[][] order)
            => string.Join(",", order.Select(o => o.ToSortQuery()));

        private static string ToFilterQuery(this object[] order)
            => order.Count() > 1 ? $"{order[0]} {order[1]}" : $"{order[0]} ASC";
        
    }
}
