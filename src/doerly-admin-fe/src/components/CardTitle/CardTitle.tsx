import { makeStyles } from 'tss-react/mui';
import React from 'react';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
    justifyContent: 'space-between',
    padding: `0px ${theme.spacing(2)}px`,
  },
}));

function CardTitle({
  children,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <div className={classes.container}>
      {children}
    </div>
  );
}

interface IProps {
  children: React.ReactNode,
}

export default CardTitle;
