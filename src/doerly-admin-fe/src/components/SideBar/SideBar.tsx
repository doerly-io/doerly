import React, { useState, useMemo, useEffect } from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import { DEFAULT_LOCATION_SEARCH } from 'components/SearchParamsConfigurator';
import Collapse from 'components/Collapse';
import Hover from 'components/Hover';
import IconCatalog from 'components/icons/Catalog';
import IconChevronDown from 'components/icons/ChevronDown';
import IconChevronUp from 'components/icons/ChevronUp';
import IconOrders from 'components/icons/Orders';
import IconPeople from 'components/icons/People';
import IconReporting from 'components/icons/Reporting';
import IconSettings from 'components/icons/Settings';
import Link from 'components/Link';
import Typography from 'components/Typography';
import useAccessValidate from 'hooks/useAccessValidate';
import useCurrentPage from 'hooks/useCurrentPage';
import useLocationSearch from 'hooks/useLocationSearch';
import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';
import authoritiesUI from 'constants/authoritiesUI';
import pagesURLs from 'constants/pagesURLs';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    backgroundColor: `${theme.sideBar.background}`,
    height: '100%',
  },
  content: {
    paddingTop: `${theme.spacing(8)}px`,
  },
  navItem: {
    alignItems: 'flex-end',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
  navItemContainer: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'space-between',
    minWidth: '200px',
    padding: `${theme.spacing(1.5)}px ${theme.spacing(2)}px`,
  },
  nestedItem: {
    alignItems: 'center',
    display: 'flex',
    padding: `${theme.spacing(1.5)}px ${theme.spacing(6)}px`,
  },
}));

const ICON_SIZE = 24;
const CHEVRON_ICON_SIZE = 20;

const allNavPages: any[] = [
  {
    icon: (<IconPeople size={ICON_SIZE} />),
    id: pages.users,
    labelIntlId: 'users',
    link: `${pagesURLs[pages.users]}`,
  },
  {
    icon: (<IconCatalog size={ICON_SIZE} />),
    id: pages.catalog,
    labelIntlId: 'catalog',
    link: `${pagesURLs[pages.catalog]}`,
  },
  {
    icon: (<IconOrders size={ICON_SIZE} />),
    id: pages.orders,
    labelIntlId: 'orders',
    link: `${pagesURLs[pages.orders]}`,
  },
  // {
  //   icon: (<IconReporting size={ICON_SIZE} />),
  //   id: pages.reporting,
  //   labelIntlId: 'reporting',
  //   link: `${pagesURLs[pages.reporting]}`,
  // },
  // {
  //   icon: (<IconSettings size={ICON_SIZE} />),
  //   id: pages.settings,
  //   labelIntlId: 'settings',
  //   link: `${pagesURLs[pages.settings]}`,
  // },
];

const neededAuthoritiesToPages: { [index: string]: string | string[] } = {
  [pages.users]: authoritiesUI.authorities.USERS_MANAGE,
  [pages.catalog]: authoritiesUI.authorities.CATALOG_MANAGE,
  [pages.orders]: authoritiesUI.authorities.ORDERS_MANAGE,
  [pages.reporting]: authoritiesUI.authorities.REPORTING_MANAGE,
  [pages.settings]: authoritiesUI.authorities.SETTINGS_MANAGE,
};

const getRootPage = (page: string) => page.split('/').shift();

const SideBar = ({
  onChange,
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const currentLocationSearch = useLocationSearch();
  const currentPage = useCurrentPage();
  const validateAccess = useAccessValidate();
  const newLocationSearch = Object
    .keys(DEFAULT_LOCATION_SEARCH)
    .reduce((acc: any, key: any) => {
      acc[key] = Object.keys(currentLocationSearch).includes(key)
        ? currentLocationSearch[key]
        : DEFAULT_LOCATION_SEARCH[key];
      return acc;
    }, {});

  const [openMenus, setOpenMenus] = useState<Record<string, boolean>>({});

  const toggleMenu = (id: string) => {
    setOpenMenus((prev) => ({
      ...prev,
      [id]: !prev[id],
    }));
  };

  const availableNavPages = useMemo(() =>
    allNavPages.map((page) => {
      if (page.children) {
        const children = page.children.filter((child: any) =>
          validateAccess(neededAuthoritiesToPages[child.id])
        );

        return children.length ? { ...page, children } : null;
      }
      return validateAccess(neededAuthoritiesToPages[page.id]) ? page : null;
    })
      .filter(Boolean),
  [validateAccess]);

  useEffect(() => {
    const isChildSelected = availableNavPages
      .some(({ children }) => (children || [])
        .some(({ id }: any) => id === currentPage));

    if (isChildSelected) {
      const parent = getRootPage(currentPage);
      if (!!parent && !openMenus[parent]) {
        setOpenMenus((prev) => ({
          ...prev,
          [parent]: true,
        }));
      }
    }
  }, [availableNavPages, currentPage]);

  return (
    <div className={classes.container}>
      <div className={classes.content}>
        {availableNavPages.map((page) => (
          <div>
            {!!page.children && (
              <>
                <Hover
                  onClick={() => toggleMenu(page.id)}
                  selected={!openMenus[page.id] && page.children
                    .some(({ id }: any) => currentPage.includes(id))}
                >
                  <div className={classes.navItemContainer}>
                    <div className={classes.navItem}>
                      {page.icon}
                      <Typography
                        color="secondary"
                        variant="subtitle"
                      >
                        <strong>
                          {formatMessage({ id: `page.${page.labelIntlId}` })}
                        </strong>
                      </Typography>
                    </div>
                    <div className={classes.navItem}>
                      {
                        openMenus[page.id]
                          ? <IconChevronUp size={CHEVRON_ICON_SIZE} />
                          : <IconChevronDown size={CHEVRON_ICON_SIZE} />
                      }
                    </div>
                  </div>
                </Hover>
                <Collapse in={openMenus[page.id]}>
                  <div>
                    {page.children.map((child: any) => (
                      <Link
                        onClick={() => onChange && onChange(page.link)}
                        to={{
                          locationSearch:
                            currentPage === child.id
                              ? currentLocationSearch
                              : newLocationSearch,
                          pathname: child.link,
                        }}
                      >
                        <Hover selected={currentPage.includes(child.id)}>
                          <div className={classes.nestedItem}>
                            <Typography
                              color="secondary"
                              variant="subtitle"
                            >
                              {formatMessage({
                                id: `page.${child.labelIntlId}` ,
                              })}
                            </Typography>
                          </div>
                        </Hover>
                      </Link>
                    ))}
                  </div>
                </Collapse>
              </>
            )}
            {!page.children && (
              <Link
                onClick={() => onChange && onChange(page.link)}
                to={{
                  locationSearch: currentPage === page.id
                    ? currentLocationSearch
                    : newLocationSearch,
                  pathname: page.link,
                }}
              >
                <Hover selected={page.id === getRootPage(currentPage)}>
                  <div className={classes.navItemContainer}>
                    <div className={classes.navItem}>
                      {page.icon}
                      <Typography
                        color="secondary"
                        variant="subtitle"
                      >
                        <strong>
                          {formatMessage({ id: `page.${page.labelIntlId}` })}
                        </strong>
                      </Typography>
                    </div>
                  </div>
                </Hover>
              </Link>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

interface IProps {
  onChange?: (input: any) => any,
}

export default SideBar;
