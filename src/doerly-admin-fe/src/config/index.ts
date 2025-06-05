const config = {
  AUTH_URL: '/auth',
  BASE_URL: process.env.REACT_APP_BASE_URL,
  CATEGORY_SERVICE: '/category',
  ORDERS_SERVICE: '/order/order',
  PROFILE_SERVICE: '/profile',
  UI_URL_PREFIX: process.env.REACT_APP_UI_URL_PREFIX || '',
};

export default config;
