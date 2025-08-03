using System.ComponentModel;

namespace JffCsharpTools.Domain.Enums
{
    /// <summary>
    /// Enum representing all Brazilian states with their official abbreviations and full names.
    /// Each state is mapped to a unique numeric identifier for database storage and reference.
    /// The Description attribute contains the full state name in Portuguese.
    /// </summary>
    public enum BrazilStatesEnum
    {
        /// <summary>Acre - Northern region state</summary>
        [Description("Acre")]
        AC = 1,
        /// <summary>Alagoas - Northeastern region state</summary>
        [Description("Alagoas")]
        AL = 2,
        /// <summary>Amapá - Northern region state</summary>
        [Description("Amapá")]
        AP = 3,
        /// <summary>Amazonas - Northern region state, largest Brazilian state</summary>
        [Description("Amazonas")]
        AM = 4,
        /// <summary>Bahia - Northeastern region state</summary>
        [Description("Bahia")]
        BA = 5,
        /// <summary>Ceará - Northeastern region state</summary>
        [Description("Ceará")]
        CE = 6,
        /// <summary>Distrito Federal - Federal District, location of capital Brasília</summary>
        [Description("Distrito Federal")]
        DF = 7,
        /// <summary>Espírito Santo - Southeastern region state</summary>
        [Description("Espírito Santo")]
        ES = 8,
        /// <summary>Goiás - Central-West region state</summary>
        [Description("Goiás")]
        GO = 9,
        /// <summary>Maranhão - Northeastern region state</summary>
        [Description("Maranhão")]
        MA = 10,
        /// <summary>Mato Grosso - Central-West region state</summary>
        [Description("Mato Grosso")]
        MT = 11,
        /// <summary>Mato Grosso do Sul - Central-West region state</summary>
        [Description("Mato Grosso do Sul")]
        MS = 12,
        /// <summary>Minas Gerais - Southeastern region state</summary>
        [Description("Minas Gerais")]
        MG = 13,
        /// <summary>Pará - Northern region state</summary>
        [Description("Pará")]
        PA = 14,
        /// <summary>Paraíba - Northeastern region state</summary>
        [Description("Paraíba")]
        PB = 15,
        /// <summary>Paraná - Southern region state</summary>
        [Description("Paraná")]
        PR = 16,
        /// <summary>Pernambuco - Northeastern region state</summary>
        [Description("Pernambuco")]
        PE = 17,
        /// <summary>Piauí - Northeastern region state</summary>
        [Description("Piauí")]
        PI = 18,
        /// <summary>Rio de Janeiro - Southeastern region state</summary>
        [Description("Rio de Janeiro")]
        RJ = 19,
        /// <summary>Rio Grande do Norte - Northeastern region state</summary>
        [Description("Rio Grande do Norte")]
        RN = 20,
        /// <summary>Rio Grande do Sul - Southern region state</summary>
        [Description("Rio Grande do Sul")]
        RS = 21,
        /// <summary>Rondônia - Northern region state</summary>
        [Description("Rondônia")]
        RO = 22,
        /// <summary>Roraima - Northern region state</summary>
        [Description("Roraima")]
        RR = 23,
        /// <summary>Santa Catarina - Southern region state</summary>
        [Description("Santa Catarina")]
        SC = 24,
        /// <summary>São Paulo - Southeastern region state, most populous Brazilian state</summary>
        [Description("São Paulo")]
        SP = 25,
        /// <summary>Sergipe - Northeastern region state, smallest Brazilian state</summary>
        [Description("Sergipe")]
        SE = 26,
        /// <summary>Tocantins - Northern region state</summary>
        [Description("Tocantins")]
        TO = 27
    }
}