import React from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';
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
  ChartType,
} from '../hooks/useStatistics';
import LineChart from 'components/LineChart/LineChart';
import Divider from 'components/Divider';

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
    height: '400px',
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
  const isSmallMobile = useIsSmallMobile();

  const {
    chartData,
    containerState,
    handleChartTypeChange,
    isFailed,
    isFetching,
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
      <Divider color={theme.colors.secondary} />
      <div className={classes.blockContainer}>
        <Breadcrumbs>
          <Breadcrumb
            label={formatMessage({ id: 'ordersStatistics' })}
            variant="text"
          />
        </Breadcrumbs>
      </div>
    </div>
  );
}

export default Statistics;
