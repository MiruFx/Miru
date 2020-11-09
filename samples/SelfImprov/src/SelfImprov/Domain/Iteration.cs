using System;
using System.Collections.Generic;
using System.Linq;
using Miru.Domain;

namespace SelfImprov.Domain
{
    public class Iteration : Entity, ITimeStamped, IBelongsToUser
    {
        public long UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public ICollection<Achievement> Achievements { get; set; } = new HashSet<Achievement>();

        public int Number { get; set; }
        
        public decimal PercentAchieved => Achievements.Count > 0 ? (GoalsAchieved / Achievements.Count) * 100 : 0;
        
        public decimal GoalsAchieved => Achievements.Count(a => a.Achieved);

        public IEnumerable<AchievementPerArea> GetAchievementsPerArea()
        {
            return Achievements
                .GroupBy(achievement => achievement.Goal.Area)
                .Select(group => new AchievementPerArea(group.Key, group))
                .ToList();
        }
    }

    public class AchievementPerArea
    {
        public Area Area { get; set; }
        public IEnumerable<Achievement> Achievements { get; set; }
        public decimal PercentAchieved { get; set; }

        public AchievementPerArea(Area area, IEnumerable<Achievement> achievements)
        {
            Area = area;
            Achievements = achievements;
            PercentAchieved = Achievements.Any() ? ((decimal) Achievements.Count(x => x.Achieved) / Achievements.Count()) * 100 : 0;
        }
    }
}