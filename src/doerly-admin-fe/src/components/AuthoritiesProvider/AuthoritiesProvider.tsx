import React, {
  createContext,
} from 'react';
import { useSelector } from 'react-redux';
import authoritiesUI from 'constants/authoritiesUI';

export const AuthoritiesContext = createContext<string[]>([]);

const AuthoritiesProvider = ({
  children,
}: IProps) => {
  // const {
  //   authorities,
  // } = useSelector(({ user }: any) => user);
  const authorities = Object.values(authoritiesUI);
  return (
    <AuthoritiesContext.Provider value={authorities}>
      {children}
    </AuthoritiesContext.Provider>
  );
};

interface IProps {
  children: React.ReactNode,
}

export default AuthoritiesProvider;
