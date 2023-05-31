﻿namespace CE_API_V2.Models.DTO
{
    public class BiomarkerDTO
    {

        public string Id { get; set; }

        public string Fieldname { get; set; }

        public string Category { get; set; }

        public BiomarkerUnitDTO[] Units { get; set; }

        public string InfoText { get; set; }

        public int OrderNumber { get; set; }

        public string PreferredUnit { get; set; }
    }
}
