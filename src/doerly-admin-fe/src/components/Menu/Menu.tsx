import React from 'react';
import MenuMUI from '@mui/material/Menu';
import { PopoverProps } from '@mui/material/Popover';
import useTheme from 'hooks/useTheme';

function Menu({
  anchorEl,
  children,
  onClose,
  open,
}: IProps) {
  const { theme } = useTheme();
  return (
    <MenuMUI
      anchorEl={anchorEl}
      onClose={onClose}
      open={open}
      sx={{
        '.MuiMenuItem-root': {
          '&:hover': {
            backgroundColor: theme.select.color.background,
          },
        },
        '.MuiPaper-root': {
          backgroundColor: theme.select.color.background,
          borderRadius: '8px',
          boxShadow: '0px 2px 10px rgba(0, 0, 0, 0.1)',
        }, 
      }}
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
