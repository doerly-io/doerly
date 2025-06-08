import React from 'react';
import TabsMui from '@mui/material/Tabs';
import useTheme from 'hooks/useTheme';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';

const Tabs = ({
  children,
  onChange,
  value,
}: IProps) => {
  const { theme } = useTheme();
  const isMobile = useIsMobile();
  const isSmallMobile = useIsSmallMobile();
  return (
    <TabsMui
      aria-label="icon label tabs example"
      onChange={onChange}
      sx={{
        '& .MuiTabs-flexContainer': {
          minWidth: 'fit-content',
        },
        '& .MuiTabs-indicator': {
          backgroundColor: `${theme.colors.cobalt}`,
          height: '3px',
        },
        '& .MuiTabs-scrollButtons': {
          color: `${theme.colors.cobalt}`,
        },
        '& .MuiTabs-scrollButtons.Mui-disabled': {
          opacity: 0.3,
        },
        background: `${theme.card.background.paper}`,
        boxShadow: '0px 0px 2px 0px rgba(0,0,0,0.2)',
        color: `${theme.colors.cobalt}`,
        minHeight: '48px',
        overflow: 'visible',
      }}
      value={value}
      variant="scrollable"
      scrollButtons="auto"
      allowScrollButtonsMobile
    >
      {children}
    </TabsMui>
  );
};

interface IProps {
  children: React.ReactNode,
  onChange: (event: React.SyntheticEvent, value: any) => void,
  value: string,
}

export default Tabs;
