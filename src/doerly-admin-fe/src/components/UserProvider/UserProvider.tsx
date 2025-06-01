import React, { createContext } from 'react';
import { useSelector } from 'react-redux';

export const UserContext = createContext<TUser>({});

export const mockUser = {
  email: 'example@gmail.com',
  id: '1',
  name: 'John Doe',
};

const UserProvider = ({
  children,
}: IProps) => {
  const {
    email: userEmail,
    id: userId,
    name: userName,
  }: any = mockUser;
  // }: any = useSelector((state: { user: TUser }) => state.user);

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
