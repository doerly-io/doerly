import {
  ERROR_SIGN_IN,
  ERROR_SIGN_OUT,
  INITIALIZE_USER,
  REQUEST_SIGN_IN,
  REQUEST_SIGN_OUT,
  SUCCESS_SIGN_IN,
  SUCCESS_SIGN_OUT,
} from '../constants/actionTypes';
import axios, { getAllErrorsMessages } from 'utils/requests';
import config from 'config';
import jwtHelper from 'utils/jwtHelper';
import storage, { keys } from 'utils/storage';

const initializeUser = () => (dispatch: any) => {
  const accessToken = storage.getItem(keys.ACCESS_TOKEN);
  if (accessToken && !jwtHelper.isTokenExpired()) {
    const userInfo = jwtHelper.getUserInfo(accessToken);
    if (userInfo) {
      dispatch(successSignIn(userInfo));
    }
  }
};

const requestSignIn = () => ({
  type: REQUEST_SIGN_IN,
});

const successSignIn = (user: any) => ({
  payload: user,
  type: SUCCESS_SIGN_IN,
});

const errorSignIn = (error: string) => ({
  payload: error,
  type: ERROR_SIGN_IN,
});

const errorSignOut = () => ({
  type: ERROR_SIGN_OUT,
});

const requestSignOut = () => ({
  type: REQUEST_SIGN_OUT,
});

const successSignOut = () => ({
  type: SUCCESS_SIGN_OUT,
});

const fetchSignIn = ({
  email,
  password,
}: any) => (dispatch: any) => {
  dispatch(requestSignIn());
  return signIn({
    email,
    password,
  }).then(({ data }) => {
    const value = data.value;
    const accessToken = value.accessToken;
    const userInfo = jwtHelper.getUserInfo(accessToken);
    storage.setItem(keys.ACCESS_TOKEN, accessToken);
    dispatch(successSignIn(userInfo));
  }).catch((error) => {
    const response = error?.response?.data;
    const errorMessage = response?.errorMessage;
    const errors = response?.errors;

    let finalErrorMessage = '';
    if (errors && typeof errors === 'object') {
      const errorMessages = getAllErrorsMessages(errors);
      finalErrorMessage = errorMessages.join(', ');
    } else {
      finalErrorMessage = errorMessage || '';
    }
    dispatch(errorSignIn(finalErrorMessage));
  });
};

const signIn = ({
  email,
  password,
}: any) => {
  const {
    AUTH_URL,
    BASE_URL,
  } = config;
  return axios.post(`${BASE_URL}${AUTH_URL}/login`, {
    email,
    password,
  });
};

const fetchSignOut = () => (dispatch: any) => {
  dispatch(requestSignOut());
  jwtHelper.removeAccessToken();
  dispatch(successSignOut());
};

const signOut = () => {
  const {
    AUTH_URL,
    BASE_URL,
  } = config;
  return axios.get(`${BASE_URL}${AUTH_URL}/logout`);
};

const exportFunctions = {
  fetchSignIn,
  fetchSignOut,
  initializeUser,
};

export default exportFunctions;
