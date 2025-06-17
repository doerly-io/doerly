import React from 'react';

const Show = ({
  children,
  condition,
}: IProps) => {
  return (condition ? <>{children}</> : null);
};

interface IProps {
  children: React.ReactNode,
  condition: boolean,
}

export default Show;
