﻿using System.ComponentModel.DataAnnotations;

namespace CE_API_V2.Models;

public class OrganizationModel
{
    [Key]
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string ContactEmail { get; set; }
}