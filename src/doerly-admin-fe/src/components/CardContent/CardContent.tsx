import { makeStyles } from 'tss-react/mui';
import React from 'react';
import classNames from 'classnames';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    display: 'flex',
    flexDirection: 'column',
    overflowX: 'auto',
  },
  padding: {
    padding: `${theme.spacing(0)}px ${theme.spacing(2)}px`,
  },
}));

function CardContent({
  children,
  disablePadding = false,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <div
      className={classNames(
        classes.container,
        disablePadding ? '' : classes.padding
      )}
    >
      {children}
    </div>
  );
}

interface IProps {
  children: React.ReactNode,
  disablePadding?: boolean,
}

export default CardContent;
