namespace NET6CoreWebAppWithbootstrap5.ExampleData
{
    public class ExampleDataModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; } = 0;

        public static IEnumerable<ExampleDataModel> DataSet = new ExampleDataModel[]
        {
            new ExampleDataModel(){ Age = 21, FirstName = "Mary", LastName = "Smith" },
            new ExampleDataModel(){ Age = 34, FirstName = "John", LastName = "Jones" },
            new ExampleDataModel(){ Age = 23, FirstName = "Barbera", LastName = "Grayson" },
            new ExampleDataModel(){ Age = 9, FirstName = "Mary", LastName = "Sue" },
            new ExampleDataModel(){ Age = 56, FirstName = "Tom", LastName = "North" }
        };
    }
}
