import React, { useEffect, useMemo, useRef, useState } from 'react';
import apiStatistics from 'api/statistics/statistics';
import { IUserActivityInfo, IPaymentStatistics } from 'api/statistics/model';
import useChangePage from 'hooks/useChangePage';
import useLocationSearch from 'hooks/useLocationSearch';
import * as tabTypes from '../constants/tabTypes';
import IconAdmin from 'components/icons/Admin';
import IconBill from 'components/icons/Bill';

const CHART_TYPES = {
  BY_DAY: 'BY_DAY',
  BY_HOUR: 'BY_HOUR',
} as const;

const PIE_CHART_TYPES = {
  OUTSTANDING_AMOUNT_BY_CURRENCY: 'OUTSTANDING_AMOUNT_BY_CURRENCY',
  PAYMENT_STATUS_DISTRIBUTION: 'PAYMENT_STATUS_DISTRIBUTION',
  PAYMENT_VOLUME_BY_CURRENCY: 'PAYMENT_VOLUME_BY_CURRENCY',
} as const;

type ChartType = typeof CHART_TYPES[keyof typeof CHART_TYPES];
type PieChartType = typeof PIE_CHART_TYPES[keyof typeof PIE_CHART_TYPES];

const DEFAULT_TAB = tabTypes.USER_ACTIVITY;

const orderedTabs = [
  tabTypes.USER_ACTIVITY,
  tabTypes.BILLS_PAYMENTS,
];

const getTabIcon = (tabType: string): React.ReactElement | null => {
  switch (tabType) {
    case tabTypes.USER_ACTIVITY:
      return <IconAdmin />;
    case tabTypes.BILLS_PAYMENTS:
      return <IconBill />;
    default:
      return null;
  }
};

const formatAmount = (amount: number): number => {
  return Number(amount.toFixed(3));
};

const useStatistics = () => {
  const componentDidMount = useRef(false);

  const changePage = useChangePage();
  const locationSearch = useLocationSearch();
  const {
    tab: selectedTab,
  } = locationSearch;

  const [containerState, setContainerState] = useState<IContainerState>({
    selectedChartType: CHART_TYPES.BY_DAY as ChartType,
    selectedPieChartType:
      PIE_CHART_TYPES.PAYMENT_VOLUME_BY_CURRENCY as PieChartType,
  });

  const [statistics, setStatistics] = useState<IStatisticsState>({
    isFailed: false,
    isFetching: false,
    paymentStatistics: null as IPaymentStatistics | null,
    userActivityInfo: null as IUserActivityInfo | null,
  });

  const availableTabs = useMemo(() => {
    return orderedTabs;
  }, []);

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

    apiStatistics.getPaymentStatistics({
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
        paymentStatistics: value as IPaymentStatistics,
      })),
    });
  };

  const handleChangeTab = (tab: string) => {
    changePage({
      locationSearch: {
        ...locationSearch,
        tab,
      },
      replace: true,
    });
  };

  const handleChartTypeChange = (chartType: ChartType) => {
    setContainerState((prev) => ({
      ...prev,
      selectedChartType: chartType,
    }));
  };

  const handlePieChartTypeChange = (pieChartType: PieChartType) => {
    setContainerState((prev) => ({
      ...prev,
      selectedPieChartType: pieChartType,
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
        }))
        .reverse();
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

  const getPieChartData = () => {
    if (!statistics.paymentStatistics) return null;

    const { 
      paymentVolumeByCurrency,
      paymentStatusDistribution,
      outstandingAmountByCurrency,
    } = statistics.paymentStatistics;

    switch (containerState.selectedPieChartType) {
      case PIE_CHART_TYPES.PAYMENT_VOLUME_BY_CURRENCY:
        return Object.entries(paymentVolumeByCurrency)
          .map(([currency, amount]) => ({
            label: currency,
            value: amount,
          }));
      case PIE_CHART_TYPES.PAYMENT_STATUS_DISTRIBUTION:
        return Object.entries(paymentStatusDistribution)
          .map(([status, count]) => ({
            label: status,
            value: count,
          }));
      case PIE_CHART_TYPES.OUTSTANDING_AMOUNT_BY_CURRENCY:
        return Object.entries(outstandingAmountByCurrency)
          .map(([currency, amount]) => ({
            label: currency,
            value: amount,
          }));
      default:
        return null;
    }
  };

  const getPaymentVolumeTrendData = () => {
    if (!statistics.paymentStatistics) return null;

    const { paymentVolumeTrend } = statistics.paymentStatistics;

    return Object.entries(paymentVolumeTrend)
      .map(([date, amount]) => ({
        label: new Date(date).toLocaleDateString(),
        value: amount,
      }))
      .reverse();
  };

  useEffect(() => {
    if (!selectedTab) {
      handleChangeTab(DEFAULT_TAB);
    }
  }, []);

  useEffect(() => {
    componentDidMount.current = true;
    fetchStatistics();
  }, []);

  return {
    availableTabs,
    chartData: getChartData(),
    containerState,
    handleChangeTab,
    handleChartTypeChange,
    handlePieChartTypeChange,
    isFailed: statistics.isFailed,
    isFetching: statistics.isFetching,
    paymentStatistics: statistics.paymentStatistics ? {
      ...statistics.paymentStatistics,
      averageBillAmount: formatAmount(
        statistics.paymentStatistics.averageBillAmount),
      averagePaymentAmount: formatAmount(
        statistics.paymentStatistics.averagePaymentAmount),
    } : null,
    paymentVolumeTrendData: getPaymentVolumeTrendData(),
    pieChartData: getPieChartData(),
    selectedTab,
    userActivityInfo: statistics.userActivityInfo,
  };
};

interface IStatisticsState {
  isFailed: boolean;
  isFetching: boolean;
  paymentStatistics: IPaymentStatistics | null;
  userActivityInfo: IUserActivityInfo | null;
}

interface IContainerState {
  selectedChartType: ChartType;
  selectedPieChartType: PieChartType;
}

export {
  CHART_TYPES,
  PIE_CHART_TYPES,
  getTabIcon
};

export type {
  ChartType,
  PieChartType
};

export default useStatistics;
