import { useEffect, useRef, useState } from 'react';
import apiStatistics from 'api/statistics/statistics';
import { IUserActivityInfo } from 'api/statistics/model';

const CHART_TYPES = {
  BY_DAY: 'BY_DAY',
  BY_HOUR: 'BY_HOUR',
} as const;

type ChartType = typeof CHART_TYPES[keyof typeof CHART_TYPES];

const useStatistics = () => {
  const componentDidMount = useRef(false);

  const [containerState, setContainerState] = useState({
    selectedChartType: CHART_TYPES.BY_DAY as ChartType,
  });

  const [statistics, setStatistics] = useState<IStatisticsState>({
    isFailed: false,
    isFetching: false,
    userActivityInfo: null as IUserActivityInfo | null,
  });

  const fetchStatistics = async () => {
    apiStatistics.getUsersActivity({
      onFailed: () => setStatistics((prev) => ({
        ...prev,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setStatistics((prev) => ({
        ...prev,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => setStatistics((prev) => ({
        ...prev,
        isFailed: false,
        isFetching: false,
        userActivityInfo: value as IUserActivityInfo,
      })),
    });
  };

  const handleChartTypeChange = (chartType: ChartType) => {
    setContainerState((prev) => ({
      ...prev,
      selectedChartType: chartType,
    }));
  };

  const getChartData = () => {
    if (!statistics.userActivityInfo) return null;

    const { 
      activityByDayLast7Days, 
      activityByHourLast24Hours,
    } = statistics.userActivityInfo;

    if (containerState.selectedChartType === CHART_TYPES.BY_DAY) {
      return Object.entries(activityByDayLast7Days)
        .map(([day, count]) => ({
          label: day,
          value: count,
        }));
    }

    return Object.entries(activityByHourLast24Hours)
      .map(([hour, count]) => ({
        label: new Date(hour)
          .toLocaleTimeString([], { 
            hour: '2-digit', 
            minute: '2-digit',
          }),
        value: count,
      }))
      .reverse();
  };

  useEffect(() => {
    componentDidMount.current = true;
    fetchStatistics();
  }, []);

  return {
    chartData: getChartData(),
    containerState,
    handleChartTypeChange,
    isFailed: statistics.isFailed,
    isFetching: statistics.isFetching,
    userActivityInfo: statistics.userActivityInfo,
  };
};

interface IStatisticsState {
  isFailed: boolean;
  isFetching: boolean;
  userActivityInfo: IUserActivityInfo | null;
}

export {
  CHART_TYPES
};

export type {
  ChartType
};

export default useStatistics;
