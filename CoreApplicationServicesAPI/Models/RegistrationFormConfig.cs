namespace CoreApplicationServicesAPI.Models
{
    public class RegistrationFormFieldOptionConfig
    {
        public string Label { get; set; } = "[Undefined]"; 
        public string Value { get; set; } = "";

    }
    public class RegistrationFormFieldConfig
    {
        public string Name { get; set; } = "Undefined";
        public string Label { get; set; } = "Undefined";
        public string Type { get; set; } = "text";
        public bool Required { get; set; }
        public RegistrationFormFieldOptionConfig[]? Options { get; set; } = null;
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

        public bool Multiple { get; set; }
        public bool UseSelect => Options != null && Options.Count()>0;
    }
    public class ApplicationAPIConfig
    {
        public string BaseUrl { get; set; } = "";
        public RegistrationFormFieldConfig[] Fields { get; set; } = new RegistrationFormFieldConfig[0];
    }
    public class RegisterUserAdditionalFieldParameters
    {
        public string Name { get; set; } = "";
        public string[]? Values { get; set; } 
    }
    public class RegisterUserParameters
    {
        public string EDIPI { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleInitial { get; set; } = "";
        public RegisterUserAdditionalFieldParameters[]? AdditionalField { get; set; }
    }
}
