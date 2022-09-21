using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NET6CoreWebAppWithbootstrap5.Models.Helpers.DataTable
{
    public interface IMappable
    {
        object MapValue(string propertyName);
    }
    public class Helper
    {
        public static Func<object?, object?> CreateGetter(PropertyInfo property)
        {
            if (!(property.DeclaringType is Type declaringType))
                throw new ArgumentNullException("property");

            var getter = property.GetGetMethod();
            if (getter == null)
                throw new ArgumentException("The specified property does not have a public accessor.");

            MethodInfo? genericMethod = typeof(Helper).GetMethod("CreateGetterGeneric");
            if(genericMethod is MethodInfo mi)
            {
                MethodInfo genericHelper = genericMethod.MakeGenericMethod(declaringType, property.PropertyType);
                var invoke = genericHelper.Invoke(null, new object?[1] { (object?)getter });
                return invoke is Func<object?, object?> method ? method : m => null;
            }
            return m => null;
        }

        public static Func<object, object?> CreateGetterGeneric<T, R>(MethodInfo getter) where T : class
        {
            Func<T, R?> getterTypedDelegate = Delegate.CreateDelegate(typeof(Func<T, R?>), getter) is Func<T, R?> val ? val : m => default;
            Func<object, object?> getterDelegate = instance => getterTypedDelegate((T)instance);
            return getterDelegate;
        }

    }
    public class PropertyInfo<T>
        where T : class
    {
        private static ConcurrentDictionary<string, Func<object?, object?>> Getters { get; set; } = new ConcurrentDictionary<string, Func<object?, object?>>();
        public static IEnumerable<PropertyInfo?> Properties => typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        public static IEnumerable<string?> PropertyNames => Properties.Where(p => p is PropertyInfo).Select(p => p is PropertyInfo prop ?  prop.Name: "");
        public static object? GetValue(T model, string? name)
        {
            if(name is string nameValue)
            {
                if (model is IMappable mappableModel)
                {
                    return mappableModel.MapValue(nameValue);
                }
                if (Getters == null)
                {
                    Getters = new ConcurrentDictionary<string, Func<object?, object?>>();
                }

                if (!Getters.ContainsKey(nameValue) && Properties.FirstOrDefault(p => p is PropertyInfo prop && prop.Name == name) is PropertyInfo propInfo)
                {
                    Getters.TryAdd(nameValue, Helper.CreateGetter(propInfo));
                }
                if (Getters.TryGetValue(nameValue, out Func<object?, object?>? getter))
                {
                    return getter is Func<object?, object?> method ? method(model) : null;
                }
            }
            
            return null;
        }
    }
    public static class DataTableExtensions
    {
        private static object? GetValue<T>(this T model, string? name) where T : class
            => PropertyInfo<T>.GetValue(model, name);
        private static string? GetValueAsString<T>(this T model, string? name) where T : class
        {
            object? value = model.GetValue(name);
            return value != null && value is DateTime date ? date.ToString("yyyy/MM/dd HH:mm:ss:fff") : value != null ? value.ToString() : "";
        }
        private static bool IsMatch(this string value, DataTableSearch settings)
        {
            return settings.Regex ? Regex.Match(value, settings.Value ?? "", RegexOptions.IgnoreCase).Success : value.ContainsCaseInsensitive(settings.Value ?? "");
        }
        private static bool ContainsCaseInsensitive(this string source, string toCheck)
        => source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        
        private static bool IsMatch<T>(this T model, DataTableColumnSearch settings, DataTableOptions<T> options) where T : class
        {
            if(options != null && options.DisplayedColumns.FirstOrDefault(c => c.Name == settings.Column.Name) is DataTableColumnOptions<T> columnOptions)
            {
                string? value = columnOptions.GetValue is Func<T,object?> getter && getter(model) is object val ? val.ToString() : null;

                string searchval = value is string str ? str : "";
                string settingsSerachVal = settings.Search is DataTableSearch search && search.Value is string s ? s : "";
                return settings.Search is DataTableSearch search2 && search2.Regex ? Regex.Match(searchval, settingsSerachVal, RegexOptions.IgnoreCase).Success :
                    searchval.ContainsCaseInsensitive(columnOptions.GetFilterValue(settingsSerachVal));
                
            }                   
            return settings.Search is DataTableSearch search3 && model.GetValueAsString(settings.Column is DataTableColumn dtCol && dtCol.Name is string name ? name: "") is string matchVal && matchVal.IsMatch(search3);
        }
        private static IEnumerable<T> GeneralSearchDataTable<T>(this IEnumerable<T> data, DataTableRequest request, DataTableOptions<T> options) where T : class
        {
            if(request.Download && request.DownloadAll)
            {
                return data;
            }            
            var searchColumns = request.GetTableSearchColumns().Where(c=>c.Search != null);
            return searchColumns.Count()> 0 ? data.Where(item => searchColumns.Any(c => item.IsMatch(c, options))): data;
        }
        private static IEnumerable<T> ColumnSpecificSearchDataTable<T>(this IEnumerable<T> data, DataTableRequest request, DataTableOptions<T> options) where T : class
        {
            if (request.Download && request.DownloadAll)
            {
                return data;
            }
            var searchColumns = request.GetSearchColumns().Where(c => c.Search != null);
            return searchColumns.Count() > 0 ? data.Where(item => searchColumns.All(c => item.IsMatch(c, options))) : data;
        }
        private static IEnumerable<T> OrderDataTable<T>(this IEnumerable<T> data, DataTableRequest request,
            DataTableOptions<T> options) where T : class
        {
            IEnumerable<DataTableColumnOrder> orderCols = request.GetOrderColumns();
            if (orderCols.Count() == 0)
            {
                return data;
            }
            IOrderedEnumerable<T>? orderedData = null;
            for (int i = 0; i < orderCols.Count(); i++)
            {
                DataTableColumnOrder col = orderCols.ElementAt(i);
                if(options._Columns.FirstOrDefault(c => c.Name == col.Name) is DataTableColumnOptions<T> colOptions)
                {
                    orderedData =
                    i == 0 && col.Dir.ToLower() == "desc" ? data.OrderByDescending(colOptions.GetValue) :
                    i == 0 ? data.OrderBy(colOptions.GetValue) :
                    col.Dir.ToLower() == "desc" && orderedData is IOrderedEnumerable<T> od ?  od.ThenByDescending(colOptions.GetValue) :
                    orderedData is IOrderedEnumerable<T> od2 ? od2.ThenBy(colOptions.GetValue): orderedData;
                }
            }
            return orderedData is IOrderedEnumerable<T> od3 ? od3: data;
        }
        private static IEnumerable<T> GetDataTablePage<T>(this IEnumerable<T> data, DataTableRequest request) where T : class
        {
            if (request.Download && request.DownloadAll)
            {
                return data;
            }
            return new ArraySegment<T>(data.ToArray(), request.Start, data.Count() - (request.Start + request.Length) < 0 ? data.Count() - request.Start : request.Length);
        }
        public static DataTableResponse ProcessDataTable<T>(
            this IEnumerable<T> data,
            DataTableRequest request,
            DataTableOptions<T> options) where T : class
        {
            try
            {
                var filteredData = data.GeneralSearchDataTable(request, options).ColumnSpecificSearchDataTable(request, options);
                var orderedData = filteredData.OrderDataTable(request, options);
                var pageData = orderedData.GetDataTablePage(request);

                var serializedData = options.GetData(pageData);
                return new DataTableResponse(request, serializedData, data.Count(), filteredData.Count(), () => options.ToSystemDataTable(pageData, options.DataName));                
            }
            catch (Exception e)
            {
                return new DataTableResponse(request, e);
            }
        }

        
    }
    public class DataTableResponse<T>
        where T : class
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<T> data { get; set; }
        public string? error { get; set; }

        public DataTableResponse(DataTableRequest request, IEnumerable<T> data, int total, int filtered)
        {
            draw = request.Draw;
            this.data = data;
            recordsTotal = total;
            recordsFiltered = filtered;
        }
        public DataTableResponse(DataTableRequest request, Exception e)
        {
            draw = request.Draw;
            data = new T[0];
            recordsTotal = 0;
            recordsFiltered = 0;
            error = e.Message;
        }

    }
    public class DataTableResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<object> data { get; set; }
        public string? error { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal Func<System.Data.DataTable> ToSystemDataTable { get; private set; }

        public DataTableResponse(DataTableRequest request, IEnumerable<object> data, int total, int filtered, Func<System.Data.DataTable> toSystemDataTable)
        {
            draw = request.Draw;
            this.data = data;
            recordsTotal = total;
            recordsFiltered = filtered;
            ToSystemDataTable = toSystemDataTable;
        }
        private static System.Data.DataTable EmptyDataTable = new System.Data.DataTable("EmptyTable"); 
        public DataTableResponse(DataTableRequest request, Exception e)
        {
            draw = request.Draw;
            data = new object[0];
            recordsTotal = 0;
            recordsFiltered = 0;
            error = e.Message;
            ToSystemDataTable = () => EmptyDataTable; 
        }

    }
    public class DataTableRequest
    {

        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public bool Download { get; set; }
        public bool DownloadAll { get; set; }
        public string? FileName { get; set; }
        public IEnumerable<DataTableOrder>? Order { get; set; }
        public IEnumerable<DataTableColumn>? Columns { get; set; }
        public DataTableSearch? Search { get; set; }
        private IEnumerable<DataTableColumn> _Columns => Columns is IEnumerable <DataTableColumn> cols ? cols : new DataTableColumn[0];
        public IEnumerable<DataTableColumnSearch> GetSearchColumns()
        => _Columns.Where(c => c.Searchable).Select(c => new DataTableColumnSearch(c));
        public IEnumerable<DataTableColumnSearch> GetTableSearchColumns()
        => _Columns.Where(c => c.Searchable).Select(c => new DataTableColumnSearch(c,Search));
        public IEnumerable<DataTableColumnOrder> GetOrderColumns()
        => Order != null ? Order.Select(o => new DataTableColumnOrder(_Columns.ElementAt(o.Column), o.Dir ?? "desc")).Where(c => c.Orderable): new DataTableColumnOrder[0];
    }
    public class DataTableRequest<T> : DataTableRequest
    where T : class
    {
        public T? Parameters { get; set; }
        public DataTableRequest() { Parameters = default(T); }
    }

    public class DataTableOrder
    {
        public int Column { get; set; }
        public string? Dir { get; set; }
    }
    public class DataTableSearch
    {
        public string? Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DataTableColumn
    {
        public Guid Guid { get; private set; }
        public string? Data { get; set; }
        public string? Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableSearch? Search { get; set; }

        public DataTableColumn()
        {
            Guid = Guid.NewGuid();
        }

    }
    public class DataTableColumnOrder
    {
        public string? Data { get; set; }
        public string? Name { get; set; }
        public string Dir { get; set; } = "desc";
        public bool Orderable { get; set; }
        public DataTableColumnOrder(DataTableColumn col, string dir)
        {
            Data = col.Data;
            Name = col.Name;
            Dir = dir;
            Orderable = col.Orderable;
        }
    }
    public class DataTableColumnSearch
    {
        public DataTableSearch? Search { get; set; }
        public DataTableColumn Column { get; set; }
        public DataTableColumnSearch(DataTableColumn col)
        {
            Column = col;
            if (col.Search != null && col.Search.Value != null)
            {
                Search = col.Search;
            }
        }
        public DataTableColumnSearch(DataTableColumn col, DataTableSearch? search)
        {
            Column = col;
            if (search is DataTableSearch s && s.Value != null)
            {
                Search = search;
            }
        }
    }

    public abstract class PropertyMapperAttribute : Attribute
    {
        private string _PropertyName { get; set; }

        public PropertyMapperAttribute(string propertyName)
        {
            _PropertyName = propertyName;
        }

        public virtual string PropertyName => _PropertyName;


    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FilterOnAttribute : PropertyMapperAttribute
    {
        public FilterOnAttribute(string propertyName) : base(propertyName) { }
    }
}