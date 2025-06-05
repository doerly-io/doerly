import React from 'react';
import DialogMui from '@mui/material/Dialog';

const Dialog = ({
  children,
  fullScreen = false,
  onClose,
  open,
  maxWidth = 'sm',
}: IProps) => {
  return (
    <DialogMui
      fullScreen={fullScreen}
      fullWidth
      onClose={onClose}
      open={open}
      maxWidth={maxWidth}
    >
      {children}
    </DialogMui>
  );
};

interface IProps {
  children: React.ReactNode,
  onClose?: (event: object, reason: string) => void,
  open: boolean,
  fullScreen?: boolean,
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl',
}

export default Dialog;
