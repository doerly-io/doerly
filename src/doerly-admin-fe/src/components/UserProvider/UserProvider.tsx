import React, { createContext } from 'react';
import { useSelector } from 'react-redux';

export const UserContext = createContext<TUser>({});

const UserProvider = ({
  children,
}: IProps) => {
  const {
    email: userEmail,
    id: userId,
    name: userName,
  } = useSelector(({ user }: any) => user);

  return (
    <UserContext.Provider
      value={{
        userEmail,
        userId,
        userName,
      }}
    >
      {children}
    </UserContext.Provider>
  );
};

interface IProps {
  children: React.ReactNode,
}

type TUser = {
  userEmail?: string,
  userId?: string,
  userName?: string,
}

export default UserProvider;
