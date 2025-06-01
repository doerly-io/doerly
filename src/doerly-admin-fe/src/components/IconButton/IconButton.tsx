import React from 'react';
import IconButtonMUI from '@mui/material/IconButton';
import useTheme from 'hooks/useTheme';

const IconButton = ({
  children,
  compact = false,
  disabled = false,
  disableHoverSpace = false,
  onClick,
}: IProps) => {
  const { theme } = useTheme();
  return (
    <IconButtonMUI
      disabled={disabled}
      onClick={onClick}
      sx={{
        boxShadow: `0px 0px 3px 0px ${theme.iconButton.border.color}`,
        height: compact ? '32px' : '40px',
        margin: disableHoverSpace ? '-8px' : '0',
        padding: '3px',
        width: compact ? '32px' : '40px',
      }}
    >
      {children}
    </IconButtonMUI>
  );
};

interface IProps {
  children: React.ReactNode,
  compact?: boolean,
  disabled?: boolean,
  disableHoverSpace?: boolean,
  onClick: (event: any) => void,
}

export default IconButton;
