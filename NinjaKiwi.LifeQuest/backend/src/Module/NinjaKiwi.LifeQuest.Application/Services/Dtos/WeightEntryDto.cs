using System;
using Abp.Application.Services.Dto;

public class WeightEntryDto : EntityDto<Guid>
{
    public Guid PersonId { get; set; }
    public float Weight { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
}