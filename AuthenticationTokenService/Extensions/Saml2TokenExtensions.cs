using AuthenticationTokenService;
using AuthenticationTokenService.Models;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace AuthenticationTokenService.Extensions
{
    public class TokenClaim
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public IEnumerable<string> Values { get; set; } = new string[0];
        public TokenClaim() { }
        internal TokenClaim(string name, IEnumerable<string> values)
        {
            Name = name;
            Values = values.Count() == 1 ? values.First().Split(',') : values;
            Value = string.Join(",", Values);
        }
        internal IEnumerable<string> GetValues() => Values != null && Values.Count() > 0 ? Values : !string.IsNullOrWhiteSpace(Value) ? Value.Split(',') : new string[0];
        internal string GetValue() => string.Join(",", GetValues());
    }
    public class AuthTokenResponse
    {
        public bool IsValid { get; set; }
        public string Value { get; set; } = string.Empty;
        public IEnumerable<TokenClaim> Claims { get; set; } = new TokenClaim[0];
        public DateTime? ExpirationDate { get; set; }
        public static AuthTokenResponse Default = new AuthTokenResponse();
        
    }
    public static class Saml2TokenExtensions
    {
        private static Saml2SecurityTokenHandler SamlTokenHandler = new Saml2SecurityTokenHandler();
        private static IEnumerable<TokenClaim> DefaultClaims = new TokenClaim[0];

        public static AuthTokenResponse ToToken(this IEnumerable<TokenClaim> claims)
        {
            var saml2Token = CreateSaml2SecurityToken(claims);
            return new AuthTokenResponse()
            {
                Value = Serialize(saml2Token),
                Claims = claims,
                ExpirationDate = saml2Token.Assertion.Conditions.NotOnOrAfter,
                IsValid = IsValidToken(saml2Token)
            };
        }
        public static AuthTokenResponse ParseToken(this string tokenStr)
        {
            if (Deserialize(tokenStr) is Saml2SecurityToken saml2Token && IsValidToken(saml2Token))
            {
                return new AuthTokenResponse()
                {
                    Value = tokenStr,
                    Claims = GetClaims(saml2Token),
                    ExpirationDate = saml2Token.Assertion.Conditions.NotOnOrAfter,
                    IsValid = IsValidToken(saml2Token)
                };
            }
            return new AuthTokenResponse()
            {
                Value = tokenStr,
                Claims = new TokenClaim[0],
                ExpirationDate = null,
                IsValid = false
            };

        }



        private static Saml2SecurityToken CreateSaml2SecurityToken(IEnumerable<TokenClaim> claims)
        {
            Saml2SubjectConfirmationData confirmationData = new Saml2SubjectConfirmationData() { Address = ApplicationConfiguration.Saml2TokenSettings.ConfirmationMethod };
            Saml2SubjectConfirmation subjectConfirmations = new Saml2SubjectConfirmation(new Uri(ApplicationConfiguration.Saml2TokenSettings.ConfirmationMethod), confirmationData);
            Saml2AudienceRestriction[] audienceRestriction = new Saml2AudienceRestriction[1] { new Saml2AudienceRestriction(ApplicationConfiguration.Saml2TokenSettings.AudienceUri is Uri uri ? uri.ToString() : "") };
            Saml2Assertion assertion = new Saml2Assertion(new Saml2NameIdentifier(ApplicationConfiguration.Saml2TokenSettings.Issuer))
            {
                Conditions = new Saml2Conditions(audienceRestriction)
                {
                    NotBefore = null,
                    NotOnOrAfter = null
                },
                InclusiveNamespacesPrefixList = ApplicationConfiguration.Saml2TokenSettings.Namespace,
                Subject = new Saml2Subject(subjectConfirmations)
                {
                    NameId = new Saml2NameIdentifier(ApplicationConfiguration.Saml2TokenSettings.ConfirmationMethod)
                }
            };


            return RenewToken(new Saml2SecurityToken(assertion), claims);
        }

        private static bool IsValidToken(Saml2SecurityToken saml2Token)
        {
            DateTime utcNow = DateTime.UtcNow;
            bool isValidIssueDate = saml2Token.Assertion.Conditions.NotBefore is DateTime issueDate && issueDate <= utcNow;
            bool isValidExpirationDate = saml2Token.Assertion.Conditions.NotOnOrAfter is DateTime expirationDate && expirationDate > utcNow;

            return isValidIssueDate && isValidExpirationDate && ApplicationConfiguration.Saml2TokenSettings.Issuer == saml2Token.Assertion.Issuer.Value;
        }
        private static IEnumerable<TokenClaim> GetClaims(Saml2SecurityToken saml2Token)
            => saml2Token.Assertion.Statements.First() is Saml2AttributeStatement saml2AttributeStatement ?
            saml2AttributeStatement.Attributes.Select(a => new TokenClaim(a.Name, a.Values)) : DefaultClaims;



        private static Saml2SecurityToken RenewToken(Saml2SecurityToken saml2Token, IEnumerable<TokenClaim> claims)
        {
            int validForFinal = ApplicationConfiguration.Saml2TokenSettings.ValidFor;
            DateTime utcNow = DateTime.UtcNow;
            saml2Token.Assertion.Conditions.NotBefore = utcNow;
            saml2Token.Assertion.Conditions.NotOnOrAfter = utcNow.AddMinutes(validForFinal);

            foreach (TokenClaim claim in claims ?? DefaultClaims)
            {
                AddUpdateSaml2Attribute(saml2Token, claim.Name, claim.GetValue());
            }
            return saml2Token;
        }

        private static string Serialize(Saml2SecurityToken token)
        {
            var sw = new System.IO.StringWriter();
            using (var xmlWriter = new System.Xml.XmlTextWriter(sw))
            {
                SamlTokenHandler.WriteToken(xmlWriter, token);
                return sw.ToString().Encrypt();
            }

        }
        private static Saml2SecurityToken? Deserialize(string tokenStr)
        {
            string decryptedValue = tokenStr.Decrypt();
            return SamlTokenHandler.CanReadToken(decryptedValue) ? SamlTokenHandler.ReadSaml2Token(decryptedValue) : null;
        }

        private static void AddUpdateSaml2Attribute(Saml2SecurityToken saml2Token, string name, object value)
        {
            if (!(saml2Token.Assertion.Statements.FirstOrDefault() is Saml2AttributeStatement))
            {
                saml2Token.Assertion.Statements.Add(new Saml2AttributeStatement());
            }
            if (saml2Token.Assertion.Statements.First() is Saml2AttributeStatement saml2AttributeStatement)
            {
                saml2AttributeStatement.Attributes.Remove(saml2AttributeStatement.Attributes.FirstOrDefault(saml2Attribute => saml2Attribute.Name == name));
                Saml2Attribute saml2AttributeReplacement = value is IEnumerable<string> stringEnumerableValue ? new Saml2Attribute(name, stringEnumerableValue) : new Saml2Attribute(name, value.ToString());
                saml2AttributeStatement.Attributes.Add(saml2AttributeReplacement);
            }
        }
    }
}
