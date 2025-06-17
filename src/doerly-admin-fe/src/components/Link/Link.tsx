import { makeStyles } from 'tss-react/mui';
import { Link as InternalLink } from 'react-router-dom';
import classNames from 'classnames';
import ExternalLink from '@mui/material/Link';
import React from 'react';
import useLocationSearch from 'hooks/useLocationSearch';

const getClasses = makeStyles()((theme) => ({
  noUnderline: {
    textDecoration: 'none',
  },
  underline: {
    '&:hover': {
      color: theme.palette.info.main,
      textDecoration: 'underline',
    },
    textDecoration: 'none',
  },
}));

function Link({
  children,
  href = '',
  onClick,
  target,
  to,
  underline = false,
}: IProps) {
  const { classes } = getClasses();
  const locationSearch = useLocationSearch();
  const LinkComponent = href
    ? ExternalLink
    : InternalLink;
  const actualTo = to
    ? `${to.pathname}?${(new URLSearchParams(to.locationSearch
      || locationSearch)).toString()}`
    : '';
  return (
    <LinkComponent
      className={classNames(
        underline ? classes.underline : classes.noUnderline
      )}
      href={href}
      onClick={onClick}
      target={target}
      to={actualTo}
    >
      {children}
    </LinkComponent>
  );
}

interface IProps {
  children: React.ReactNode,
  href?: string,
  onClick?: (input: any) => any,
  target?: '_blank' | '_self' | '_parent' | '_top',
  to?: {
    locationSearch?: any,
    pathname: string,
  },
  underline?: boolean,
}

export default Link;
