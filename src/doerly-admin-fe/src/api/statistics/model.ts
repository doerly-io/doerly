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

export enum ECurrency {
  UAH = 1, // Ukrainian Hryvnia
  USD = 2, // US Dollar
  EUR = 3, // Euro
}

export enum EPaymentStatus {
  Pending = 1,
  Failed = 2,
  Error = 3,
  Completed = 10,
}

export interface IPaymentStatistics {
  // Payment Volume Metrics
  totalPaymentVolume: number;
  averagePaymentAmount: number;
  paymentVolumeByCurrency: Record<ECurrency, number>;
  paymentVolumeTrend: Record<string, number>;

  // Payment Status Analytics
  paymentStatusDistribution: Record<EPaymentStatus, number>;

  // Bill Analytics
  totalOutstandingAmount: number;
  totalOutstandingBills: number;
  averageBillAmount: number;
  outstandingAmountByCurrency: Record<ECurrency, number>;
}
