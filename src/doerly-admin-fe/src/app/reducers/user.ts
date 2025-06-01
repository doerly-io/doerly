import authoritiesUI from 'constants/authoritiesUI';
import { mockUser } from 'components/UserProvider/UserProvider';

export const mockUserState = {
  authorities: Object.values(authoritiesUI),
  email: mockUser.email,
  errorType: '',
  id: mockUser.id,
  isAuthorized: true,
  isFailedFetchUser: false,
  isFailedSignIn: false,
  isFailedSignOut: false,
  isFetchingSignOut: false,
  isFetchingUser: false,
  name: mockUser.name,
};
