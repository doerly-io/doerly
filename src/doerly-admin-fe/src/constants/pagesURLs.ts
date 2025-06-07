import * as pages from './pages';
import config from 'config';

const result = {
  [pages.defaultPage]: `${config.UI_URL_PREFIX}/${pages.defaultPage}`,
  [pages.catalog]: `${config.UI_URL_PREFIX}/${pages.catalog}`,
  [pages.catalogFilters]: `${config.UI_URL_PREFIX}/${pages.catalogFilters}`,
  [pages.login]: `${config.UI_URL_PREFIX}/${pages.login}`,
  [pages.orders]: `${config.UI_URL_PREFIX}/${pages.orders}`,
  [pages.reporting]: `${config.UI_URL_PREFIX}/${pages.reporting}`,
  [pages.settings]: `${config.UI_URL_PREFIX}/${pages.settings}`,
  [pages.users]: `${config.UI_URL_PREFIX}/${pages.users}`,
};

export default result;
