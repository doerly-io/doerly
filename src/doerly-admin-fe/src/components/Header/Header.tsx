import React, { useMemo, useRef, useState } from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import { useSelector } from 'react-redux';
import { DEFAULT_LOCATION_SEARCH } from '../SearchParamsConfigurator';
import Hover from 'components/Hover';
import IconButton from 'components/IconButton';
import IconGlobe from 'components/icons/Globe';
import IconMenu from 'components/icons/Menu';
import Link from 'components/Link';
import Logo from 'components/Logo';
import Menu from 'components/Menu';
import MenuItem from 'components/MenuItem';
import Typography from 'components/Typography';
import useChangePage from 'hooks/useChangePage';
import useCurrentPage from 'hooks/useCurrentPage';
import useIsMobile from 'hooks/useIsMobile';
import useLocationSearch from 'hooks/useLocationSearch';
import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';
import languages, { TLanguages } from 'constants/languages';
import pagesURLs from 'constants/pagesURLs';
import SwipeableDrawer from '../SwipeableDrawer';
import SideBar from '../SideBar';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    background: theme.header.background,
    boxShadow: '0px 0px 6px 0px',
    display: 'flex',
    height: `${theme.header.height}px`,
    zIndex: 1300,
  },
  content: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'space-between',
    padding: `${theme.spacing(1)}px ${theme.spacing(2)}px`,
    width: '100%',
  },
  hover: {
    padding: `${theme.spacing(0.5)}px ${theme.spacing(1)}px`,
  },
  selectedLang: {
    display: 'flex',
    width: 'fit-content',
  },
  toolBarContainerLeft: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
  toolBarContainerRight: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    justifyContent: 'flex-end',
  },
  userNameMobile: {
    maxWidth: '100px',
  },
}));

const ICON_SIZE = 24;
const ICON_SIZE_DEFAULT = 32;

const interfaceLagsTranslate: TLanguages = {
  [languages.en]: 'English',
  [languages.ua]: 'Українська',
};

const interfaceLagsTranslateShort: TLanguages = {
  [languages.en]: 'Eng',
  [languages.ua]: 'Укр',
};

const orderedInterfaceLangs = [languages.ua, languages.en];

const rightPanelItemTypes = {
  LANGUAGE: 'language',
  LOGIN: 'login',
  SEPARATOR: 'separator',
  USER_NAME: 'userName',
};

function Header({ onLogout }: IProps) {
  const { theme } = useTheme();
  const { formatMessage } = useIntl();
  const changePage = useChangePage();
  const { classes } = getClasses(theme);
  const currentPage = useCurrentPage();
  const isMobile = useIsMobile();
  const langsMenuRef = useRef(null);
  const locationSearch = useLocationSearch();
  const user = useSelector(({ user: reducerUser }: any) => reducerUser);
  const userMenuRef = useRef(null);

  const [state, setState] = useState({
    isLangsMenuOpened: false,
    isNavMenuOpened: false,
    isUserMenuOpened: false,
  });

  const defaultLocationSearch = Object
    .keys(DEFAULT_LOCATION_SEARCH)
    .reduce((acc: any, key: any) => {
      acc[key] = Object.keys(locationSearch).includes(key)
        ? locationSearch[key]
        : DEFAULT_LOCATION_SEARCH[key];
      return acc;
    }, {});

  const actualOrderedRightPanelItemTypes = useMemo(() => {
    const result = [];
    if (user.isAuthorized) {
      result.push(rightPanelItemTypes.USER_NAME);
    } else if (
      !user.isFetchingUser
      && currentPage !== pages.login
    ) {
      result.push(rightPanelItemTypes.LOGIN);
    }
    result.push(rightPanelItemTypes.LANGUAGE);
    return result.reduce((acc: any, item, index) => {
      if (index > 0) {
        acc.push(rightPanelItemTypes.SEPARATOR);
      }
      acc.push(item);
      return acc;
    }, []);
  }, [user, currentPage]);

  return (
    <div className={classes.container}>
      <div className={classes.content}>
        <div className={classes.toolBarContainerLeft}>
          {(user.isAuthorized && isMobile) && (
            <>
              <IconButton
                compact
                onClick={() => setState({
                  ...state,
                  isNavMenuOpened: !state.isNavMenuOpened,
                })}
              >
                <IconMenu
                  color="header"
                  size={ICON_SIZE}
                />
              </IconButton>
              <SwipeableDrawer
                anchor="left"
                isOpened={state.isNavMenuOpened}
                onClose={() => setState({
                  ...state,
                  isNavMenuOpened: false,
                })}
              >
                <SideBar
                  onChange={() => setState({
                    ...state,
                    isNavMenuOpened: false,
                  })}
                />
              </SwipeableDrawer>
            </>
          )}
          <Link
            onClick={() => setState({
              ...state,
              isNavMenuOpened: false,
            })}
            to={{
              locationSearch: defaultLocationSearch,
              pathname: `${pagesURLs[pages.defaultPage]}`,
            }}
          >
            <Hover light>
              <div className={classes.hover}>
                <Logo compact={isMobile} />
              </div>
            </Hover>
          </Link>
        </div>
        <div className={classes.toolBarContainerRight}>
          {actualOrderedRightPanelItemTypes.map((itemType: string) => (
            <>
              {itemType === rightPanelItemTypes.USER_NAME && (
                <div ref={userMenuRef}>
                  <Hover
                    light
                    onClick={() => setState({
                      ...state,
                      isUserMenuOpened: true,
                    })}
                    selected={state.isUserMenuOpened}
                  >
                    <div className={classes.hover}>
                      <div
                        className={isMobile ? classes.userNameMobile : ''}
                      >
                        <Typography
                          noWrap
                          variant="subtitle"
                        >
                          {!isMobile
                            ? (
                              <strong>
                                {user.name}
                              </strong>
                            )
                            : user.name
                          }
                        </Typography>
                      </div>
                    </div>
                  </Hover>
                </div>
              )}
              {itemType === rightPanelItemTypes.LOGIN && (
                <Link
                  to={{
                    pathname: `${pagesURLs[pages.login]}`,
                  }}
                >
                  <Hover
                    light
                    selected={currentPage === pages.login}
                  >
                    <div className={classes.hover}>
                      <Typography variant="subtitle">
                        <strong>
                          {formatMessage({ id: 'signIn' })}
                        </strong>
                      </Typography>
                    </div>
                  </Hover>
                </Link>
              )}
              {itemType === rightPanelItemTypes.LANGUAGE && (
                <>
                  <div className={classes.selectedLang}>
                    <Typography noWrap>
                      {(isMobile
                        ? interfaceLagsTranslateShort
                        : interfaceLagsTranslate
                      )[locationSearch.lang]}
                    </Typography>
                  </div>
                  <div ref={langsMenuRef}>
                    <IconButton
                      compact={isMobile}
                      onClick={() => setState({
                        ...state,
                        isLangsMenuOpened: true,
                      })}
                    >
                      <IconGlobe
                        color="header"
                        size={isMobile ? ICON_SIZE : ICON_SIZE_DEFAULT}
                      />
                    </IconButton>
                  </div>
                </>
              )}
              {itemType === rightPanelItemTypes.SEPARATOR && (
                <Typography variant="subtitle">
                  <strong>|</strong>
                </Typography>
              )}
            </>
          ))}
        </div>
        <Menu
          anchorEl={langsMenuRef.current}
          open={state.isLangsMenuOpened}
          onClose={() => setState({
            ...state,
            isLangsMenuOpened: false,
          })}
        >
          {orderedInterfaceLangs.map(lang => (
            <MenuItem
              onClick={() => {
                changePage({
                  locationSearch: {
                    ...locationSearch,
                    lang,
                  },
                  replace: true,
                });
                setState({
                  ...state,
                  isLangsMenuOpened: false,
                });
              }}
              selected={locationSearch.lang === lang}
            >
              <Typography>
                {interfaceLagsTranslate[lang]}
              </Typography>
            </MenuItem>
          ))}
        </Menu>
        <Menu
          anchorEl={userMenuRef.current}
          open={state.isUserMenuOpened}
          onClose={() => setState({
            ...state,
            isUserMenuOpened: false,
          })}
        >
          <MenuItem
            onClick={() => {
              setState({
                ...state,
                isUserMenuOpened: false,
              });
              onLogout();
            }}
          >
            <Typography>
              {formatMessage({ id: 'signOut' })}
            </Typography>
          </MenuItem>
        </Menu>
      </div>
    </div>
  );
}

interface IProps {
  onLogout: () => void,
}

export default Header;
