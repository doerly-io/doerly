import React from 'react';

const symbol = ':';
const Colon = ({ children }: { children: React.ReactNode }) => {
  return <>{children}{symbol}</>;
};

export default Colon;
