import React, { useEffect, useState } from 'react';
import { makeStyles } from 'tss-react/mui';
import {
  BrowserRouter,
  Routes,
  Route,
} from 'react-router-dom';
import AuthoritiesProvider from 'components/AuthoritiesProvider';
import CustomThemeProvider from 'components/CustomThemeProvider';
import Header from 'components/Header';
import IntlProvider from 'components/IntlProvider';
import PageDefault from 'pageProviders/Default';
import pageURLs from 'constants/pagesURLs';
import SearchParamsConfigurator from 'components/SearchParamsConfigurator';
import SideBar from 'components/SideBar';
import useIsMobile from 'hooks/useIsMobile';
import UserProvider from 'components/UserProvider';
import useTheme from 'hooks/useTheme';

import * as pages from 'constants/pages';


const getClasses = makeStyles<any>()((_) => ({
  sideBarContainer: {
    position: 'absolute',
  },
}));

function App() {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const isMobile = useIsMobile();

  const [state, setState] = useState({
    componentDidMount: false,
  });

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
                <Header onLogout={() => {
                }}/>
                {!isMobile && (
                  <div className={classes.sideBarContainer}>
                    <SideBar/>
                  </div>
                )}
                <Routes>
                  <Route
                    element={<PageDefault/>}
                    path={`${pageURLs[pages.defaultPage]}`}
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
