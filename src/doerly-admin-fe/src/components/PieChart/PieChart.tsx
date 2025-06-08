import React from 'react';
import {
  PieChart as RechartsPieChart,
  Pie,
  Cell,
  ResponsiveContainer,
  Tooltip,
  Legend,
} from 'recharts';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    height: '100%',
    width: '100%',
  },
  label: {
    fill: theme.typography.color.primary,
    fontSize: '12px',
  },
}));

interface IPieChartProps {
  data: Array<{
    label: string;
    value: number;
  }>;
}

const COLORS = [
  '#76fbc8',
  '#6fedff',
  '#ffe19c',
  '#FF8042',
  '#8884D8',
];

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({
  cx,
  cy,
  midAngle,
  innerRadius,
  outerRadius,
  percent,
  index,
  label,
  value,
}: any) => {
  const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
  const x = cx + radius * Math.cos(-midAngle * RADIAN);
  const y = cy + radius * Math.sin(-midAngle * RADIAN);

  return (
    <text
      x={x}
      y={y}
      fontWeight={600}
      fontSize={18}
      textAnchor={x > cx ? 'start' : 'end'}
      dominantBaseline="central"
    >
      {`${(percent * 100).toFixed(0)}%`}
      <tspan x={x} dy="1.2em" textAnchor={x > cx ? 'start' : 'end'}>
        {`(${value.toFixed(2)})`}
      </tspan>
    </text>
  );
};

function PieChart({ data }: IPieChartProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <div className={classes.container}>
      <ResponsiveContainer>
        <RechartsPieChart>
          <Pie
            data={data}
            dataKey="value"
            nameKey="label"
            cx="50%"
            cy="50%"
            fill="#8884d8"
            label={renderCustomizedLabel}
            labelLine={false}
          >
            {data.map((entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>
          <Tooltip
            contentStyle={{
              backgroundColor: theme.select.color.focus,
              borderColor: theme.select.color.background,
              color: theme.select.color.label,
            }}
            formatter={(value: number) => value.toFixed(2)}
          />
          <Legend
            layout="horizontal"
            verticalAlign="bottom"
            align="center"
          />
        </RechartsPieChart>
      </ResponsiveContainer>
    </div>
  );
}

export default PieChart;
