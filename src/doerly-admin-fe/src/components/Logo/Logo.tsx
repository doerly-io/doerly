import { makeStyles } from 'tss-react/mui';
import React from 'react';
import IconAdmin from 'components/icons/Admin';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
  label: {
    color: '#34d399',
    fontFamily: '"Segoe UI", sans-serif',
    fontSize: 20,
    fontWeight: 400,
    lineHeight: 1.5,
  },
  labelCompactContainer: {
    display: 'flex',
    flexDirection: 'column',
  },
  labelCompactMain: {
    color: '#34d399',
    fontFamily: '"Segoe UI", sans-serif',
    fontSize: 16,
  },
  labelCompactSub: {
    color: '#34d399',
    fontFamily: '"Segoe UI", sans-serif',
    fontSize: 12,
  },
}));

function Logo({
  compact = false,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <div className={classes.container}>
      {!compact && <IconAdmin color="header"/>}
      {!compact && (
        <div className={classes.label}>
          <strong>
            Doerly Admin
          </strong>
        </div>
      )}
      {compact && (
        <div className={classes.labelCompactContainer}>
          <div className={classes.labelCompactMain}>
            <strong>
              Doerly
            </strong>
          </div>
          <div className={classes.labelCompactSub}>
            Admin
          </div>
        </div>
      )}
    </div>
  );
}

interface IProps {
  compact?: boolean,
}

export default Logo;
