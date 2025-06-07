namespace Doerly.Module.Authorization.Contracts.Responses;

public class UserActivityStatisticsDto
{
    public int ActiveUsersLast24Hours { get; set; }
    public int ActiveUsersLast7Days { get; set; }
    public int ActiveUsersLast30Days { get; set; }
    public Dictionary<DateTime, int> ActivityByHourLast24Hours { get; set; }
    public Dictionary<DayOfWeek, int> ActivityByDayLast7Days { get; set; }
    public int TotalUsersCount { get; set; }
    public int InactiveUsersLast30Days { get; set; }
    public int NewUsersLast30Days { get; set; }
}