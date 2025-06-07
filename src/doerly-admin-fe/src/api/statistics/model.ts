export interface IUserActivityInfo {
  activeUsersLast24Hours: number;
  activeUsersLast7Days: number;
  activeUsersLast30Days: number;
  activityByHourLast24Hours: Record<string, number>;
  activityByDayLast7Days: Record<string, number>;
  totalUsersCount: number;
  inactiveUsersLast30Days: number;
  newUsersLast30Days: number;
}
