import React from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import useIsMobile from 'hooks/useIsMobile';
// import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';
import pagesURLs from 'constants/pagesURLs';
import Typography from 'components/Typography';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: `${theme.spacing(3)}px`,
  },
  containerMobile: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(3)}px`,
  },
  itemContainer: {
    display: 'flex',
    height: '200px',
    width: '275px',
  },
  itemContainerMobile: {
    display: 'flex',
    height: '200px',
  },
  itemContent: {
    alignItems: 'center',
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
    justifyContent: 'center',
    padding: `${theme.spacing(3)}px`,
    width: '100%',
  },
}));

const ICON_SIZE = 40;

const items: any[] = [];


function Dashboard() {
  // const { theme } = useTheme();
  // const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const isMobile = useIsMobile();

  return (
    <div>
      <Typography variant="title">
        Default Page
      </Typography>
    </div>
  );
}

export default Dashboard;
