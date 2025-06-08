import React from 'react';
import TabMui from '@mui/material/Tab';
import useTheme from 'hooks/useTheme';

const Tab = ({
  disabled = false,
  icon,
  label,
  value,
  ...props
}: IProps) => {
  const { theme } = useTheme();
  return (
    <TabMui
      disabled={disabled}
      icon={icon}
      iconPosition="start"
      label={label}
      sx={{
        '&.Mui-selected': {
          backgroundColor: `${theme.colors.secondary}`,
          color: `${theme.colors.black}`,
        },
        '&.MuiTab-root': {
          gap: `${theme.spacing(1)}px`,
        },
        color: `${theme.colors.cobalt}`,
      }}
      value={value}
      {...props}
    />
  );
};

interface IProps {
  disabled?: boolean,
  icon?: any,
  label: React.ReactNode,
  value: string,
}

export default Tab;
