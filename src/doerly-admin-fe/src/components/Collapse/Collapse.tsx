import React from 'react';
import CollapseMUI from '@mui/material/Collapse';

const Collapse = ({
  children,
  in: inProp,
  timeout = 'auto',
  unmountOnExit = true,
}: IProps) => {
  return (
    <CollapseMUI
      in={inProp}
      timeout={timeout}
      unmountOnExit={unmountOnExit}
    >
      {children}
    </CollapseMUI>
  );
};

interface IProps {
  children: React.ReactNode;
  in: boolean;
  timeout?: 'auto' | number;
  unmountOnExit?: boolean;
}

export default Collapse;
