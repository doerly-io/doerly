import React from 'react';
import {
  BarChart as RechartsBarChart,
  Bar,
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
    width: '100%',
  },
}));

interface IBarChartProps {
  data: Array<{
    label: string;
    value: number;
  }>;
}

function BarChart({ data }: IBarChartProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <div className={classes.container}>
      <ResponsiveContainer>
        <RechartsBarChart
          data={data}
          margin={{
            bottom: 5,
            left: 20,
            right: 30,
            top: 20,
          }}
        >
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis
            dataKey="label"
            tick={{ fill: theme.typography.color.primary }}
          />
          <YAxis
            tick={{ fill: theme.typography.color.primary }}
          />
          <Tooltip
            contentStyle={{
              backgroundColor: theme.card.background.paper,
              borderColor: theme.select.color.background,
              color: theme.colors.secondary,
            }}
            formatter={(value: number) => value.toFixed(2)}
          />
          <Bar
            dataKey="value"
            fill={theme.colors.secondary}
            radius={[4, 4, 0, 0]}
          />
        </RechartsBarChart>
      </ResponsiveContainer>
    </div>
  );
}

export default BarChart; 
