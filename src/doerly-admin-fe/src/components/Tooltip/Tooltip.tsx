import { ClickAwayListener } from '@mui/material';
import TooltipMui from '@mui/material/Tooltip';
import React, { useState } from 'react';
import { makeStyles } from 'tss-react/mui';
import useIsMobile from 'hooks/useIsMobile';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
  },
}));

type TPlacement = 'bottom-end'
  | 'bottom-start'
  | 'bottom'
  | 'left-end'
  | 'left-start'
  | 'left'
  | 'right-end'
  | 'right-start'
  | 'right'
  | 'top-end'
  | 'top-start'
  | 'top';

const Tooltip = ({
  arrow = false,
  children,
  placement = 'bottom',
  title,
}: IProps) => {
  const { theme } = useTheme();
  const isMobile = useIsMobile();
  const { classes } = getClasses(theme);
  const [open, setOpen] = useState(false);

  const handleClick = () => {
    if (isMobile) {
      setOpen(prev => !prev);
    }
  };

  const handleClose = () => {
    if (isMobile) {
      setOpen(false);
    }
  };

  return (
    <ClickAwayListener onClickAway={handleClose}>
      <TooltipMui
        arrow={arrow}
        disableInteractive
        title={title}
        placement={placement}
        open={isMobile ? open : undefined}
        onClose={handleClose}
        disableHoverListener={isMobile}
        disableFocusListener={isMobile}
        disableTouchListener={isMobile}
      >
        <div className={classes.container} onClick={handleClick}>
          {children}
        </div>
      </TooltipMui>
    </ClickAwayListener>
  );
};

interface IProps {
  arrow?: boolean,
  children: React.ReactNode,
  placement?: TPlacement,
  title: React.ReactNode,
}

export default Tooltip;
