import React, { useEffect, useState } from 'react';
import { Dispatch } from 'redux';
import { makeStyles } from 'tss-react/mui';
import {
  useDispatch,
  useSelector,
} from 'react-redux';
import {
  BrowserRouter,
  Routes,
  Route,
} from 'react-router-dom';
import AuthoritiesProvider from 'components/AuthoritiesProvider';
import CustomThemeProvider from 'components/CustomThemeProvider';
import Header from 'components/Header';
import IntlProvider from 'components/IntlProvider';
import Loading from 'components/Loading';
import PageContainer from 'components/PageContainer';
import PageDefault from 'pageProviders/Default';
import PageLogin from 'pageProviders/Login';
import PageUsers from 'pageProviders/Users';
import PageCatalog from 'pageProviders/Catalog';
import PageOrders from 'pageProviders/Orders';
import PageReporting from 'pageProviders/Reporting';
import PageSettings from 'pageProviders/Settings';
import pageURLs from 'constants/pagesURLs';
import SearchParamsConfigurator from 'components/SearchParamsConfigurator';
import SideBar from 'components/SideBar';
import useIsMobile from 'hooks/useIsMobile';
import UserProvider from 'components/UserProvider';
import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';
import userActions from '../actions/user';
import { addAxiosInterceptors } from 'utils/requests';


const getClasses = makeStyles<any>()((_) => ({
  sideBarContainer: {
    position: 'absolute',
  },
}));

function App() {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const isMobile = useIsMobile();
  const dispatch: Dispatch<any> = useDispatch();

  const [state, setState] = useState({
    componentDidMount: false,
  });
  const {
    isFetchingUser,
  } = useSelector(({ user }: any) => user);

  useEffect(() => {
    addAxiosInterceptors({
      onSignOut: () => dispatch(userActions.fetchSignOut()),
    });
    dispatch(userActions.initializeUser());
    setState({
      ...state,
      componentDidMount: true,
    });
  }, []);

  return (
    <UserProvider>
      <AuthoritiesProvider>
        <CustomThemeProvider>
          <BrowserRouter>
            <SearchParamsConfigurator/>
            {state.componentDidMount && (
              <IntlProvider>
                <Header onLogout={() => dispatch(userActions.fetchSignOut())}/>
                {!isMobile && (
                  <div className={classes.sideBarContainer}>
                    <SideBar/>
                  </div>
                )}
                {isFetchingUser && (
                  <PageContainer>
                    <Loading />
                  </PageContainer>
                )}
                {!isFetchingUser && (
                  <Routes>
                    <Route
                      element={<PageDefault/>}
                      path={`${pageURLs[pages.defaultPage]}`}
                    />
                    <Route
                      element={(
                        <PageLogin/>
                      )}
                      path={`${pageURLs[pages.login]}`}
                    />
                    <Route
                      element={(
                        <PageUsers/>
                      )}
                      path={`${pageURLs[pages.users]}`}
                    />
                    <Route
                      element={(
                        <PageCatalog/>
                      )}
                      path={`${pageURLs[pages.catalog]}`}
                    />
                    <Route
                      element={(
                        <PageOrders/>
                      )}
                      path={`${pageURLs[pages.orders]}`}
                    />
                    <Route
                      element={(
                        <PageReporting/>
                      )}
                      path={`${pageURLs[pages.reporting]}`}
                    />
                    <Route
                      element={(
                        <PageSettings/>
                      )}
                      path={`${pageURLs[pages.settings]}`}
                    />
                    <Route
                      element={<PageDefault/>}
                      path="*"
                    />
                  </Routes>
                )}
              </IntlProvider>
            )}
          </BrowserRouter>
        </CustomThemeProvider>
      </AuthoritiesProvider>
    </UserProvider>
  );
}

export default App;
