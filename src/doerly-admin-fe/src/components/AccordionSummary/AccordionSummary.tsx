import React from 'react';
import AccordionSummaryMui from '@mui/material/AccordionSummary';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  paddingNone: {
    padding: '0px',
  },
}));

const AccordionSummary = ({
  children,
  disablePadding = false,
  expandIcon,
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <AccordionSummaryMui
      className={disablePadding ? classes.paddingNone : ''}
      expandIcon={expandIcon}
    >
      {children}
    </AccordionSummaryMui>
  );
};

interface IProps {
  children: React.ReactNode,
  disablePadding?: boolean,
  expandIcon?: React.ReactNode,
}

export default AccordionSummary;
