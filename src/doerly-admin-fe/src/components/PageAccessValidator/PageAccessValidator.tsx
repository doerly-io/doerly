import React, { useEffect, useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { mockUserState } from 'app/reducers/user';
import useAccessValidate from 'hooks/useAccessValidate';
import useChangePage from 'hooks/useChangePage';
import useLocationSearch from 'hooks/useLocationSearch';
import * as pages from 'constants/pages';
import pagesURLs from 'constants/pagesURLs';
import AuthModes from 'constants/authoritiesValidationModes';

function  PageAccessValidator({
  neededAuthorities = [],
  children,
  mode = AuthModes.ANY,
}: IProps) {

  const changePage = useChangePage();
  const location = useLocation();
  const locationSearch = useLocationSearch();
  const validateAccess = useAccessValidate();

  const [state, setState] = useState({
    isValid: false,
  });

  const {
    isFetchingUser,
    isAuthorized,
  } = mockUserState;
  // } = useSelector(({ user }: any) => user);

  const hasAccess = useMemo(
    () => validateAccess(neededAuthorities, mode),
    [neededAuthorities, validateAccess]
  );

  useEffect(() => {
    if (!isFetchingUser) {
      if (!isAuthorized) {
        changePage({
          locationSearch: {
            ...locationSearch,
            redirectLocationSearch: locationSearch.redirectLocationSearch
                || JSON.stringify(locationSearch),
            redirectPathname: locationSearch.redirectPathname
                || location.pathname,
          },
          pathname: `${pagesURLs[pages.login]}`,
          replace: true,
        });
      } else if (!hasAccess) {
        changePage({
          pathname: `${pagesURLs[pages.defaultPage]}`,
        });
      } else {
        setState(prevState => ({
          ...prevState,
          isValid: true,
        }));
      }
    }
  }, [isFetchingUser, isAuthorized, hasAccess]);

  return (
    <>
      {state.isValid ? children : null}
    </>
  );
}

interface IProps {
  children: React.ReactNode,
  neededAuthorities?: string | string[],
  mode?: AuthModes,
}

export default PageAccessValidator;
