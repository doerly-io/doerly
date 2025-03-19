import {
  useLocation,
} from 'react-router-dom';
import config from 'config';

const useCurrentPage = () => {
  const {
    pathname,
  } = useLocation();

  return pathname
    .substring(pathname.indexOf(config.UI_URL_PREFIX || '')
      + (config.UI_URL_PREFIX?.length || 0) + 1);
};

export default useCurrentPage;
