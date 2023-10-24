﻿namespace CE_API_V2.Models.DTO
{
    public class BiomarkersLocalized
    {
        public string Id { get; set; }
        public string Fieldname { get; set; }
        public string InfoText { get; set; }
        public Dictionary<string, string> DisplayNames { get; set; }
    }

    public class BiomarkersLocalizedNew
    {
        public string[] Categories { get; set; }
        public MedicalHistoryLocalized[] MedicalHistory { get; set; }
        public LabResultLocalized[] LabResults { get; set; }
    }
    
    public class BiomarkersBaseLocalized
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string InfoText { get; set; }
        public string Category { get; set; }
    }

    public class LabResultLocalized : BiomarkersBaseLocalized
    {
        public BiomarkerSchemaUnit[] Units { get; set; }
    }

    public class MedicalHistoryLocalized : BiomarkersBaseLocalized
    {
        public BiomarkerSchemaUnit Unit { get; set; }
    }
}
