import React from 'react';
import AccordionDetailsMui from '@mui/material/AccordionDetails';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  paddingNone: {
    padding: '0px',
  },
}));

const AccordionDetails = ({
  children,
  disablePadding = false,
  variant = 'paper',
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <AccordionDetailsMui
      className={disablePadding ? classes.paddingNone : ''}
      sx={{
        backgroundColor: theme.card.background[variant],
      }}
    >
      {children}
    </AccordionDetailsMui>
  );
};

export type AccordionDetailsVariant =
  | 'paper'
  | 'edit'
  | 'error'
  | 'info'
  | 'success'
  | 'warning';

interface IProps {
  children: React.ReactNode,
  disablePadding?: boolean,
  variant?: AccordionDetailsVariant,
}

export default AccordionDetails;
