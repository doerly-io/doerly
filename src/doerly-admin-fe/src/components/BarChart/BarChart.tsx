import React from 'react';
import { makeStyles } from 'tss-react/mui';
import {
  BarChart as RechartsBarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts';
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

const BarChart = ({ data }: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <div className={classes.container}>
      <ResponsiveContainer>
        <RechartsBarChart
          data={data}
          margin={{
            bottom: 5,
            left: 5,
            right: 5,
            top: 5,
          }}
        >
          <CartesianGrid
            strokeDasharray="3 3"
            stroke={theme.colors.cobalt}
          />
          <XAxis
            dataKey="label"
            stroke={theme.colors.cobalt}
          />
          <YAxis
            stroke={theme.colors.cobalt}
          />
          <Tooltip
            contentStyle={{
              backgroundColor: theme.card.background.paper,
              border: `1px solid ${theme.colors.cobalt}`,
              borderRadius: `${theme.spacing(0.5)}px`,
              color: theme.colors.cobalt,
            }}
            cursor={{ fill: theme.colors.cobalt + '20' }}
          />
          <Bar
            dataKey="value"
            fill={theme.colors.secondary}
          />
        </RechartsBarChart>
      </ResponsiveContainer>
    </div>
  );
};

export default BarChart; 
