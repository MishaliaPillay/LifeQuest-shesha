using System;
using System.Collections.Generic;

public class UpdateHealthPathDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Guid> MealPlanIds { get; set; }
    public List<Guid> WeightEntryIds { get; set; }
}