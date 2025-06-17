import React from 'react';
import {
  LineChart as RechartsLineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    height: '100%',
    overflow: 'hidden',
    width: '100%',
  },
}));

interface IDataPoint {
  label: string;
  value: number;
}

interface IProps {
  data: IDataPoint[];
}

const LineChart = ({ data }: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <div className={classes.container}>
      <ResponsiveContainer>
        <RechartsLineChart
          data={data}
          margin={{
            bottom: 5,
            left: 5,
            right: 5,
            top: 5,
          }}
        >
          <CartesianGrid
            stroke={theme.colors.cobalt + '20'}
            strokeDasharray="3 3"
          />
          <XAxis
            dataKey="label"
            stroke={theme.colors.cobalt}
            tick={{ fill: theme.colors.cobalt }}
          />
          <YAxis
            stroke={theme.colors.cobalt}
            tick={{ fill: theme.colors.cobalt }}
          />
          <Tooltip
            contentStyle={{
              backgroundColor: theme.colors.background,
              border: `1px solid ${theme.colors.cobalt}`,
              borderRadius: `${theme.spacing(0.5)}px`,
              color: theme.colors.cobalt,
            }}
            cursor={{ stroke: theme.colors.cobalt + '20' }}
          />
          <Line
            dataKey="value"
            dot={{
              fill: theme.colors.secondary,
              stroke: theme.colors.secondary,
              strokeWidth: 2,
            }}
            stroke={theme.colors.secondary}
            strokeWidth={2}
            type="monotone"
          />
        </RechartsLineChart>
      </ResponsiveContainer>
    </div>
  );
};

export default LineChart;
