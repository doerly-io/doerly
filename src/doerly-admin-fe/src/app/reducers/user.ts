import {
  ERROR_SIGN_IN,
  ERROR_SIGN_OUT,
  REQUEST_SIGN_IN,
  REQUEST_SIGN_OUT,
  SUCCESS_SIGN_IN,
  SUCCESS_SIGN_OUT,
} from '../constants/actionTypes';
import authoritiesUI from 'constants/authoritiesUI';

const initialState = {
  authorities: '',
  email: '',
  errors: '',
  id: 0,
  isAuthorized: false,
  isFailedSignIn: false,
  isFailedSignOut: false,
  isFetchingSignIn: false,
  isFetchingSignOut: false,
  name: '',
};

export default function Reducer(state = initialState, action: Action) {
  switch (action.type) {
    case ERROR_SIGN_IN: {
      const errors = action.payload;
      return {
        ...state,
        errors: errors || '',
        isFailedSignIn: true,
        isFetchingSignIn: false,
      };
    }

    case REQUEST_SIGN_IN: {
      return {
        ...state,
        errors: '',
        isFailedSignIn: false,
        isFetchingSignIn: true,
      };
    }

    case SUCCESS_SIGN_IN: {
      const {
        email: userEmail,
        id: userId,
        role,
      } = action.payload;
      const authorities = authoritiesUI.roleAuthorities[role] || [];
      return {
        ...state,
        authorities: authorities,
        email: userEmail,
        id: userId,
        isAuthorized: true,
        isFailedSignIn: false,
        isFetchingSignIn: false,
        name: userEmail,
      };
    }

    case ERROR_SIGN_OUT: {
      return {
        ...state,
        isFailedSignOut: true,
        isFetchingSignOut: false,
      };
    }

    case REQUEST_SIGN_OUT: {
      return {
        ...state,
        errors: '',
        isFailedSignOut: false,
        isFetchingSignOut: true,
      };
    }

    case SUCCESS_SIGN_OUT: return initialState;

    default:
      return state;
  }
};
