import React from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import useIsMobile, { useIsDesktop, useIsSmallMobile } from 'hooks/useIsMobile';
import useTheme from 'hooks/useTheme';
import Breadcrumb from 'components/Breadcrumb';
import Breadcrumbs from 'components/Breadcrumbs';
import Card from 'components/Card';
import CardContent from 'components/CardContent';
import CardTitle from 'components/CardTitle';
import Typography from 'components/Typography';
import Loading from 'components/Loading';
import Show from 'components/Show';
import Select from 'components/Select';
import MenuItem from 'components/MenuItem';
import useStatistics, {
  CHART_TYPES,
  PIE_CHART_TYPES,
  ChartType,
  PieChartType,
  getTabIcon,
} from '../hooks/useStatistics';
import LineChart from 'components/LineChart/LineChart';
import BarChart from 'components/BarChart/BarChart';
import PieChart from 'components/PieChart/PieChart';
import Tabs from 'components/Tabs';
import Tab from 'components/Tab';
import * as tabTypes from '../constants/tabTypes';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  blockContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(3)}px`,
  },
  chartContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
    height: '60vh',
    overflow: 'hidden',
    width: '100%',
  },
  chartTypeSelector: {
    minWidth: '150px',
  },
  container: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(5)}px`,
  },
  gridContainer: {
    display: 'grid',
    gap: `${theme.spacing(3)}px`,
    gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))',
  },
  gridContainerMobile: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(3)}px`,
  },
  statsCard: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
  },
  statsValue: {
    fontSize: '24px',
    fontWeight: 600,
  },
}));

const ICON_SIZE = 24;

type Colors = 'primary' | 'secondary' | 'success' | 'error';

function Statistics() {
  const { theme } = useTheme();
  const { classes, cx } = getClasses(theme);
  const { formatMessage } = useIntl();
  const isMobile = useIsMobile();
  const isDesktop = useIsDesktop();
  const isSmallMobile = useIsSmallMobile();

  const {
    availableTabs,
    chartData,
    containerState,
    handleChangeTab,
    handleChartTypeChange,
    handlePieChartTypeChange,
    isFailed,
    isFetching,
    paymentStatistics,
    paymentVolumeTrendData,
    pieChartData,
    selectedTab,
    userActivityInfo,
  } = useStatistics();

  const renderStatsCard = (
    title: string,
    value: number | string,
    color?: Colors
  ) => (
    <Card>
      <CardContent>
        <div className={classes.statsCard}>
          <Typography color="secondary">
            {title}
          </Typography>
          <Typography
            color={color}
            variant="title"
          >
            {value}
          </Typography>
        </div>
      </CardContent>
    </Card>
  );

  return (
    <div className={classes.container}>
      <Tabs
        onChange={(event, tab) => handleChangeTab(tab)}
        value={selectedTab}
      >
        {availableTabs.map((tab: any) => (
          <Tab
            label={formatMessage({ id: `tab.${tab}` })}
            icon={getTabIcon(tab)}
            value={tab}
          />
        ))}
      </Tabs>
      <Show condition={selectedTab === tabTypes.USER_ACTIVITY}>
        <div className={classes.blockContainer}>
          <Breadcrumbs>
            <Breadcrumb
              label={formatMessage({ id: 'userActivity' })}
              variant="text"
            />
          </Breadcrumbs>

          <Show condition={isFetching}>
            <Loading variant="loading" />
          </Show>

          <Show condition={!isFetching && !!userActivityInfo}>
            <div className={cx(
              classes.gridContainer,
              isSmallMobile && classes.gridContainerMobile
            )}>
              {renderStatsCard(
                formatMessage({ id: 'statistics.totalUsers' }),
                userActivityInfo?.totalUsersCount || 0,
                'primary'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.inactiveUsers30d' }),
                userActivityInfo?.inactiveUsersLast30Days || 0,
                'error'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.newUsers30d' }),
                userActivityInfo?.newUsersLast30Days || 0,
                'primary'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.activeUsers24h' }),
                userActivityInfo?.activeUsersLast24Hours || 0,
                'success'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.activeUsers7d' }),
                userActivityInfo?.activeUsersLast7Days || 0,
                'success'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.activeUsers30d' }),
                userActivityInfo?.activeUsersLast30Days || 0,
                'success'
              )}
            </div>

            <Card>
              <CardTitle>
                <Typography variant="title">
                  {formatMessage({ id: 'statistics.activityChart' })}
                </Typography>
                <div className={classes.chartTypeSelector}>
                  <Select
                    onChange={({ target }) => handleChartTypeChange(
                      target.value as ChartType
                    )}
                    value={containerState.selectedChartType}
                  >
                    <MenuItem value={CHART_TYPES.BY_DAY}>
                      {formatMessage({ id: 'statistics.chartType.byDay' })}
                    </MenuItem>
                    <MenuItem value={CHART_TYPES.BY_HOUR}>
                      {formatMessage({ id: 'statistics.chartType.byHour' })}
                    </MenuItem>
                  </Select>
                </div>
              </CardTitle>
              <CardContent>
                <div className={classes.chartContainer}>
                  <Show condition={!!chartData}>
                    <LineChart data={chartData || []} />
                  </Show>
                </div>
              </CardContent>
            </Card>
          </Show>
        </div>
      </Show>
      <Show condition={selectedTab === tabTypes.BILLS_PAYMENTS}>
        <div className={classes.blockContainer}>
          <Breadcrumbs>
            <Breadcrumb
              label={formatMessage({ id: 'ordersStatistics' })}
              variant="text"
            />
          </Breadcrumbs>

          <Show condition={isFetching}>
            <Loading variant="loading" />
          </Show>

          <Show condition={!isFetching && !!paymentStatistics}>
            <div className={cx(
              classes.gridContainer,
              isSmallMobile && classes.gridContainerMobile
            )}>
              {renderStatsCard(
                formatMessage({ id: 'statistics.totalPaymentVolume' }),
                paymentStatistics?.totalPaymentVolume || 0,
                'primary'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.averagePaymentAmount' }),
                paymentStatistics?.averagePaymentAmount || 0,
                'primary'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.averageBillAmount' }),
                paymentStatistics?.averageBillAmount || 0,
                'primary'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.totalOutstandingAmount' }),
                paymentStatistics?.totalOutstandingAmount || 0,
                'error'
              )}
              {renderStatsCard(
                formatMessage({ id: 'statistics.totalOutstandingBills' }),
                paymentStatistics?.totalOutstandingBills || 0,
                'error'
              )}

            </div>

            <div
              className={!isDesktop
                ? classes.gridContainerMobile
                : classes.gridContainer}
            >
              <Card>
                <CardTitle>
                  <Typography variant="title">
                    {formatMessage({ id: 'statistics.paymentDistribution' })}
                  </Typography>
                  <div className={classes.chartTypeSelector}>
                    <Select
                      contentMinWidth={300}
                      onChange={({ target }) => handlePieChartTypeChange(
                        target.value as PieChartType
                      )}
                      value={containerState.selectedPieChartType}
                    >
                      <MenuItem value={
                        PIE_CHART_TYPES.PAYMENT_VOLUME_BY_CURRENCY}>
                        {formatMessage({
                          id: 'statistics.pieChartType.paymentVolumeByCurrency',
                        })}
                      </MenuItem>
                      <MenuItem value={
                        PIE_CHART_TYPES.PAYMENT_STATUS_DISTRIBUTION}>
                        {formatMessage({
                          id:
                            'statistics.pieChartType.paymentStatusDistribution',
                        })}
                      </MenuItem>
                      <MenuItem value={
                        PIE_CHART_TYPES.OUTSTANDING_AMOUNT_BY_CURRENCY}
                      >
                        {formatMessage({
                          id:
                          // eslint-disable-next-line max-len
                            'statistics.pieChartType.outstandingAmountByCurrency',
                        })}
                      </MenuItem>
                    </Select>
                  </div>
                </CardTitle>
                <CardContent>
                  <div className={classes.chartContainer}>
                    <Show condition={!!pieChartData}>
                      <PieChart data={pieChartData || []} />
                    </Show>
                  </div>
                </CardContent>
              </Card>

              <Card>
                <CardTitle>
                  <Typography variant="title">
                    {formatMessage({
                      id: 'statistics.paymentVolumeTrend' })}
                  </Typography>
                </CardTitle>
                <CardContent>
                  <div className={classes.chartContainer}>
                    <Show condition={!!paymentVolumeTrendData}>
                      <BarChart data={paymentVolumeTrendData || []} />
                    </Show>
                  </div>
                </CardContent>
              </Card>
            </div>
          </Show>
        </div>
      </Show>
    </div>
  );
}

export default Statistics;
