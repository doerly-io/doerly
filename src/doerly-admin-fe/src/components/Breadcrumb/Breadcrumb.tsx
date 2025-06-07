import { makeStyles } from 'tss-react/mui';
import Hover from 'components/Hover';
import IconButton from 'components/IconButton';
import Link from 'components/Link';
import React from 'react';
import Typography from 'components/Typography';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    height: '32px',
  },
  link: {
    alignItems: 'center',
    color: theme.link.color,
    display: 'flex',
    height: '32px',
  },
}));

type TVariants = 'link' | 'text';

function Breadcrumb({
  children,
  compact,
  Icon,
  label,
  to,
  variant,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <div className={classes.container}>
      {!!Icon && (
        <Link to={to}>
          <IconButton onClick={() => ({})}>
            <Icon color={theme.breadcrumb.color} size={24} />
          </IconButton>
        </Link>
      )}
      {variant === 'link' && (
        <Hover>
          <Link
            to={to}
          >
            <div className={classes.link}>
              <Typography
                color="inherit"
                variant={compact ? 'subtitle' : 'title'}
              >
                <strong>
                  {label}
                </strong>
              </Typography>
            </div>
          </Link>
        </Hover>
      )}
      {variant === 'text' && (
        <Typography
          color="secondary"
          variant={compact ? 'subtitle' : 'title'}
        >
          <strong>
            {label}
          </strong>
        </Typography>
      )}
    </div>
  );
}

interface IProps {
  children?: React.ReactNode,
  compact?: boolean,
  Icon?: any,
  label?: string,
  to?: {
    locationSearch?: any,
    pathname: string,
  },
  variant: TVariants,
}

export default Breadcrumb;
