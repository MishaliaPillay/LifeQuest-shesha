using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{  /// <summary>
   /// Represents a Path in LifeQuest ( FitnessPath, HealthPath)
   /// </summary>
    [Entity(TypeShortAlias = "LifQu.HealthPath")]
    public class HealthPath : Path
    {
        public virtual ICollection<MealPlan> MealPlans { get; set; } = new List<MealPlan>();

        public virtual ICollection<WeightEntry> WeightEntries { get; set; } = new List<WeightEntry>();
    }
}
