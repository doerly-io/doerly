import { useState } from 'react';
import { Dispatch } from 'redux';
import { useDispatch, useSelector } from 'react-redux';
import actionsAuth from 'app/actions/user';

const errorTypes = {
  EMPTY_FIELD_LOGIN: 'EMPTY_FIELD_LOGIN',
  EMPTY_FIELD_PASSWORD: 'EMPTY_FIELD_PASSWORD',
};

const ErrorTypesBE = {
  EMAIL: 'email',
};

const useLogin = () => {
  const dispatch: Dispatch<any> = useDispatch();
  const {
    errors,
    isFetchingSignIn,
    isFailedSignIn,
  } = useSelector(({ user }: any) => user);

  const [state, setState] = useState({
    validationErrors: [] as string[],
  });

  const [login, setLogin] = useState({
    email: '',
    password: '',
  });

  const fetchSighIn = () => {
    const validationErrors = getValidationErrors();
    if (!validationErrors.length) {
      dispatch(actionsAuth.fetchSignIn({
        email: login.email,
        password: login.password,
      }));
    }
    setState((prevState) => ({
      ...prevState,
      validationErrors,
    }));
  };

  const getValidationErrors = () => {
    const errors = [];
    if (!login.email.trim()) {
      errors.push(errorTypes.EMPTY_FIELD_LOGIN);
    }
    if (!login.password.trim()) {
      errors.push(errorTypes.EMPTY_FIELD_PASSWORD);
    }
    return errors;
  };

  const handleChangeEmail = (email: string) => {
    setLogin(prevState => ({
      ...prevState,
      email,
    }));
  };

  const handleChangePassword = (password: string) => {
    setLogin(prevState => ({
      ...prevState,
      password,
    }));
  };

  const handleSignIn = () => {
    fetchSighIn();
  };

  return {
    email: login.email,
    errorsMsg: errors || '',
    handleChangeEmail,
    handleChangePassword,
    handleSignIn,
    isFailedSignIn,
    isFetchingSignIn,
    password: login.password,
    validationErrors: state.validationErrors,
  };
};

export {
  errorTypes
};

export default useLogin;
