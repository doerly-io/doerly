import React, { useEffect, useMemo, useState, useRef } from 'react';
import { makeStyles } from 'tss-react/mui';
import classNames from 'classnames';
import useIsMobile from 'hooks/useIsMobile';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  bottomStub: {
    height: '16px',
  },
  container: {
    background: theme.pageContainer.container.background,
    display: 'flex',
    height: '100%',
    overflowY: 'auto',
  },
  content: {
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
  },
  contentContainer: {
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    justifyContent: 'center',
    overflowX: 'auto',
    scrollbarGutter: 'stable',
    width: '100%',
  },
  contentMinWidth: {
    minWidth: theme.pageContainer.content.width,
  },
  fullWidth: {
    maxWidth: '100% !important',
  },
  innerContent: {
    height: '100%',
    padding: `${theme.spacing(2)}px`,
  },
  leftSideBar: {
    minWidth: `${theme.sideBar.width}px`,
    width: '20%',
  },
  leftSideBarDisabled: {
    width: '20%',
  },
  leftSideBarDisabledForFullWidth: {
    width: '0px',
  },
  leftSideBarForFullWidth: {
    minWidth: `${theme.sideBar.width}px`,
  },
  rightSideBar: {
    width: '20%',
  },
  rightSideBarZeroWidth: {
    width: '0px !important',
  },
}));

function PageContainer({
  children,
  fullWidth = false,
  sideBarStub = true,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const isMobile = useIsMobile();
  const contentRef = useRef<HTMLDivElement>(null);
  const rightSideBarRef = useRef<HTMLDivElement>(null);

  const [state, setState] = useState({
    hasRightSideBar: !isMobile && !fullWidth,
  });

  const sideBarStubClass = useMemo(() => {
    if (!sideBarStub) {
      return fullWidth
        ? classes.leftSideBarDisabledForFullWidth
        : classes.leftSideBarDisabled;
    }
    return fullWidth
      ? classes.leftSideBarForFullWidth
      : classes.leftSideBar;
  }, [sideBarStub, fullWidth]);

  useEffect(() => {
    const element = rightSideBarRef.current;
    if (element) {
      const resizeObserver = new ResizeObserver(([entry]) => {
        if (entry.contentRect.width === 0) {
          setState((prevState) => ({
            ...prevState,
            hasRightSideBar: false,
          }));
        }
      });
      resizeObserver.observe(element);
      return () => {
        resizeObserver.unobserve(element);
      };
    }
  }, [isMobile, fullWidth]);

  useEffect(() => {
    const element = contentRef.current;
    if (element) {
      const resizeObserver = new ResizeObserver(([entry]) => {
        if (entry.contentRect.width > theme.pageContainer.content.width) {
          setState((prevState) => ({
            ...prevState,
            hasRightSideBar: !isMobile && !fullWidth,
          }));
        }
      });
      resizeObserver.observe(element);
      return () => {
        resizeObserver.unobserve(element);
      };
    }
  }, [isMobile, fullWidth]);

  return (
    <div className={classes.container}>
      <div className={classes.contentContainer}>
        {!isMobile && (
          <div className={sideBarStubClass} />
        )}
        <div
          className={classNames(
            classes.content,
            sideBarStub && state.hasRightSideBar && classes.contentMinWidth,
            (isMobile || fullWidth) && classes.fullWidth
          )}
          ref={contentRef}
        >
          <div className={classes.innerContent}>
            {children}
            <div className={classes.bottomStub} />
          </div>
        </div>
        {!isMobile && !fullWidth && (
          <div
            className={classNames(
              classes.rightSideBar,
              sideBarStub && !state.hasRightSideBar
              && classes.rightSideBarZeroWidth
            )}
            ref={rightSideBarRef}
          />
        )}
      </div>
    </div>
  );
}

interface IProps {
  children: React.ReactNode,
  fullWidth?: boolean,
  sideBarStub?: boolean,
}

export default PageContainer;
