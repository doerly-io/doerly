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
import pageURLs from 'constants/pagesURLs';
import SearchParamsConfigurator from 'components/SearchParamsConfigurator';
import SideBar from 'components/SideBar';
import useIsMobile from 'hooks/useIsMobile';
import UserProvider from 'components/UserProvider';
import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';
import userActions from '../actions/user';


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
                <Routes>
                  <Route
                    element={<PageDefault/>}
                    path={`${pageURLs[pages.defaultPage]}`}
                  />
                  <Route
                    element={(
                      <PageLogin />
                    )}
                    path={`${pageURLs[pages.login]}`}
                  />
                  <Route
                    element={<PageDefault/>}
                    path="*"
                  />
                </Routes>
              </IntlProvider>
            )}
          </BrowserRouter>
        </CustomThemeProvider>
      </AuthoritiesProvider>
    </UserProvider>
  );
}

export default App;
