import React from 'react';
import SnackbarMUI from '@mui/material/Snackbar';

const Snackbar = ({
  autoHide = false,
  anchorOrigin = {
    horizontal: 'center',
    vertical: 'top',
  },
  children,
  onClose,
  open = false,
}: IProps) => {
  return (
    <SnackbarMUI
      anchorOrigin={anchorOrigin}
      autoHideDuration={autoHide ? 3000 : null}
      onClose={onClose}
      open={open}
      sx={anchorOrigin.vertical === 'top'
        ? { top: '60px !important' }
        : { bottom: '40px !important' }}
      transitionDuration={{ exit: 0 }}
    >
      <div>
        {children}
      </div>
    </SnackbarMUI>
  );
};

interface IProps {
  anchorOrigin?: {
    horizontal: 'center' | 'left' | 'right',
    vertical: 'bottom' | 'top',
  }
  autoHide?: boolean,
  onClose?: (event: React.SyntheticEvent | Event, reason: string) => void,
  open: boolean,
  children: React.ReactElement,
}

export default Snackbar;
