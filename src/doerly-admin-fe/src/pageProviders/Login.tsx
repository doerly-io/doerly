import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import useChangePage from 'hooks/useChangePage';
import LoginPage from 'pages/login';
import PageContainer from 'components/PageContainer';
import useLocationSearch from 'hooks/useLocationSearch';
import * as pages from 'constants/pages';
import pagesURLs from 'constants/pagesURLs';

const Login = (props: any) => {
  const locationSearch = useLocationSearch();
  const user = useSelector(({ user }: any) => user);
  const changePage = useChangePage();
  useEffect(() => {
    if (user.isAuthorized) {
      changePage({
        locationSearch: locationSearch.redirectLocationSearch
          ? JSON.parse(locationSearch.redirectLocationSearch)
          : locationSearch,
        pathname: locationSearch.redirectPathname
          || `${pagesURLs[pages.defaultPage]}`,
        replace: true,
      });
    }
  }, [user.isAuthorized]);

  return (
    <PageContainer sideBarStub={false}>
      {user.isAuthorized
        ? null
        : (
          <LoginPage {...props} />
        )}
    </PageContainer>
  );
};

export default Login;
