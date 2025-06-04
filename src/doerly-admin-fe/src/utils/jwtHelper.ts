import storage, { keys } from './storage';

const PAYLOAD_ID_KEY =
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
const PAYLOAD_EMAIL_KEY =
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
const PAYLOAD_ROLE_KEY =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

interface UserInfo {
  id: number;
  email: string;
  role: string;
}

const setAccessToken = (token: string): void => {
  storage.setItem(keys.ACCESS_TOKEN, token);
};

const isLoggedIn = (): boolean => {
  return !isTokenExpired();
};

const isTokenExpired = (): boolean => {
  const accessToken = storage.getItem(keys.ACCESS_TOKEN);
  if (!accessToken) {
    return true;
  }
  const payload = getPayload(accessToken);
  return payload.exp < Date.now() / 1000;
};

const getUserInfo = (
  token: string | null
): UserInfo | null => {
  const accessToken = token
    ? token
    : storage.getItem(keys.ACCESS_TOKEN);
  if (!accessToken) {
    return null;
  }
  const payload = getPayload(accessToken);
  return {
    email: payload[PAYLOAD_EMAIL_KEY],
    id: payload[PAYLOAD_ID_KEY],
    role: payload[PAYLOAD_ROLE_KEY],
  } as UserInfo;
};

const getPayload = (token: string): any => {
  try {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  } catch (error) {
    removeAccessToken();
  }
};

const removeAccessToken = (): void => {
  storage.removeItem(keys.ACCESS_TOKEN);
};

const functionsToExport = {
  getUserInfo,
  removeAccessToken,
  setAccessToken,
};

export default functionsToExport;



