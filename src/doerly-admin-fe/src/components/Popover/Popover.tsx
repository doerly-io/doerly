import React from 'react';
import PopoverMUI, { PopoverProps } from '@mui/material/Popover';

const Popover = ({
  anchorEl,
  anchorOrigin = { horizontal: 'left', vertical: 'bottom' },
  children,
  onClose,
  open,
  transformOrigin = { horizontal: 'left', vertical: 'top' },
}: IProps) => {
  return (
    <PopoverMUI
      anchorEl={anchorEl}
      anchorOrigin={anchorOrigin}
      onClose={onClose}
      open={open}
      transformOrigin={transformOrigin}
    >
      {children}
    </PopoverMUI>
  );
};

interface IProps {
  anchorEl: PopoverProps['anchorEl'],
  anchorOrigin?: {
    horizontal: 'center' | 'left' | 'right' | number,
    vertical: 'bottom' | 'center' | 'top' | number,
  },
  children: React.ReactElement,
  onClose?: (event: any) => void,
  open: boolean,
  transformOrigin?: {
    horizontal: 'center' | 'left' | 'right' | number,
    vertical: 'bottom' | 'center' | 'top' | number,
  },
}

export default Popover;
