import React from 'react';
import MenuItemMUI from '@mui/material/MenuItem';

import MenuItemContent from './MenuItemContent';
import useTheme from 'hooks/useTheme';

function MenuItem({
  children,
  disabled,
  onClick,
  searchValues,
  selected = false,
  value,
}: IProps) {
  const { theme } = useTheme();
  return (
    <MenuItemMUI
      disabled={disabled}
      onClick={onClick}
      selected={selected}
      value={value}
      sx={{
        '&.Mui-selected': {
          '&:hover': {
            backgroundColor: theme.select.color.background,
          },
          backgroundColor: theme.select.color.background,
        },
        '&:hover': {
          backgroundColor: theme.select.color.background,
        },
        backgroundColor: theme.select.color.background,
        color: theme.select.color.label,
      }}
    >
      <MenuItemContent>
        {children}
      </MenuItemContent>
    </MenuItemMUI>
  );
}

MenuItem.doerlyAdminName = 'MenuItem';

interface IProps {
  children: React.ReactNode,
  disabled?: boolean,
  onClick?: (event: any) => void,
  // example: searchValues=[label, label2]. This way search will works by any
  // matches from label or label2
  searchValues?: Array<string>,
  selected?: boolean,
  value?: any,
}

export default MenuItem;
