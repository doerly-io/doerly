import React from 'react';
import IconButtonMUI from '@mui/material/IconButton';

const IconButton = ({
  children,
  compact = false,
  disabled = false,
  disableHoverSpace = false,
  onClick,
  visible = true,
}: IProps) => {
  return (
    <IconButtonMUI
      disabled={disabled}
      onClick={onClick}
      sx={{
        height: compact ? '32px' : '40px',
        margin: disableHoverSpace ? '-8px' : '0',
        padding: '3px',
        visibility: visible ? 'visible' : 'hidden',
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
  visible?: boolean,
}

export default IconButton;
