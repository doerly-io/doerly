import React from 'react';
import CardMUI from '@mui/material/Card';
import useTheme from 'hooks/useTheme';

function Card({
  customBackground,
  children,
  disableGap = false,
  disablePaddings = false,
  variant = 'paper',
}: IProps) {
  const { theme } = useTheme();
  return (
    <CardMUI
      sx={{
        background: customBackground || theme.card.background[variant],
        borderRadius: '0px',
        display: 'flex',
        flexDirection: 'column',
        gap: disableGap ? '0px' : `${theme.spacing(2)}px`,
        overflow: 'initial',
        padding: disablePaddings
          ? 'none'
          : `${theme.spacing(2)}px 0px`,
        transition: 'all 0.2s ease-out',
        width: '100%',
      }}
    >
      {children}
    </CardMUI>
  );
}

interface IProps {
  customBackground?: string,
  children: React.ReactNode,
  disableGap?: boolean,
  disablePaddings?: boolean,
  variant?: 'paper'
    | 'edit'
    | 'error'
    | 'info'
    | 'success'
    | 'warning'
}

export default Card;
