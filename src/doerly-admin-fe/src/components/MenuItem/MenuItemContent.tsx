import React from 'react';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  content: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    width: '100%',
  },
}));

function MenuItemContent({ children }: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  return (
    <div className={classes.content}>
      {children}
    </div>
  );
}

MenuItemContent.doerlyAdminName = 'MenuItemContent';

interface IProps {
  children: React.ReactNode,
}

export default MenuItemContent;
