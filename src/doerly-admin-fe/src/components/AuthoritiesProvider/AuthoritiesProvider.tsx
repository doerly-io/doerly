import React, {
  createContext,
} from 'react';
import { useSelector } from 'react-redux';

export const AuthoritiesContext = createContext<string[]>([]);

const AuthoritiesProvider = ({
  children,
}: IProps) => {
  const {
    authorities,
  } = useSelector(({ user }: any) => user);
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
