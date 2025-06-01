import React from 'react';
import MenuMUI from '@mui/material/Menu';
import { PopoverProps } from '@mui/material/Popover';

function Menu({
  anchorEl,
  children,
  onClose,
  open,
}: IProps) {
  return (
    <MenuMUI
      anchorEl={anchorEl}
      onClose={onClose}
      open={open}
    >
      {children}
    </MenuMUI>
  );
}

interface IProps {
  anchorEl: PopoverProps['anchorEl'],
  children: React.ReactNode,
  onClose: (event: any) => void,
  open: boolean,
}

export default Menu;
