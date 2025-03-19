import { useLocation, useNavigate } from 'react-router-dom';
import useLocationSearch from 'hooks/useLocationSearch';

interface IChangePageArgs {
  locationSearch?: any,
  pathname?: string,
  replace?: boolean,
}

function useChangePage() {
  const locationSearch = useLocationSearch();
  const location = useLocation();
  const navigate = useNavigate();
  return ({
    locationSearch: inputLocationSearch,
    pathname,
    replace = false,
  }: IChangePageArgs) => {
    navigate(
      {
        pathname: pathname || location.pathname,
        search: `?${new URLSearchParams(inputLocationSearch || locationSearch)
          .toString()}`,
      },
      {
        replace,
      }
    );
  };
}

export default useChangePage;
