import axios from 'axios';
import config from 'config';
import storage from 'utils/storage';

axios.interceptors.request.use((params) => ({
  ...params,
  withCredentials: 'withCredentials' in params
    ? params.withCredentials
    : true,
}));

const signOut = (store: any) => {
  storage.removeItem('isAuthorized');
  return Promise.resolve();
};
// commented out until the correct logout is implemented
/*  return axios.post(
    `${config.BASE_URL}${config.AUTH_SERVICE}/logout`,
    null
  ).finally(() => {
    storage.removeItem('isAuthorized');
    store.dispatch({
      type: SUCCESS_SIGN_OUT,
    });
  });
};*/

const addReduxInterceptors = (store: any) => {
  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      switch (error.response?.status) {
        case 401: {
          signOut(store);
          return Promise.reject(error);
        }
        default: {
          return Promise.reject(error);
        }
      }
    }
  );
};

export {
  addReduxInterceptors
};

export default axios;
