const config = {
  AUTH_URL: '/auth',
  BACKEND_URL: process.env.REACT_APP_BACKEND_URL,
  BASE_URL: process.env.REACT_APP_BASE_URL,
  CATALOG_SERVICE: '/Catalog',
  CATEGORY_SERVICE: '/category',
  ORDERS_SERVICE: '/order/order',
  PROFILE_SERVICE: '/profile',
  STATISTICS_SERVICE: '/statistics',
  UI_URL_PREFIX: process.env.REACT_APP_UI_URL_PREFIX || '',
};

export default config;
