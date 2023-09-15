﻿using System.ComponentModel.DataAnnotations;

namespace CE_API_V2.Models;

public class CountryModel
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ContactEmail { get; set; }
}