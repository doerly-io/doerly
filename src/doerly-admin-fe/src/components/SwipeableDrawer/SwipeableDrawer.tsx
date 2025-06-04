import React from 'react';
import SwipeableDrawerMUI from '@mui/material/SwipeableDrawer';

const SwipeableDrawer = ({
  anchor = 'left',
  children,
  isOpened,
  onClose,
}: IProps) => {
  return (
    <SwipeableDrawerMUI
      anchor={anchor}
      disableSwipeToOpen
      disableDiscovery
      disableBackdropTransition
      open={isOpened}
      onClose={onClose}
      onOpen={() => ({})}
    >
      {children}
    </SwipeableDrawerMUI>
  );
};

interface IProps {
  anchor?: 'left' | 'top' | 'right' | 'bottom',
  children: React.ReactNode,
  isOpened: boolean,
  onClose: (input: any) => any,
}

export default SwipeableDrawer;
