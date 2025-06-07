import React from 'react';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
    justifyContent: 'flex-end',
    padding: `${theme.spacing(0)}px ${theme.spacing(2)}px`,
  },
}));

function CardActions({
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

export default CardActions;
