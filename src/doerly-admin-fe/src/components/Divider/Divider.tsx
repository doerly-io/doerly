import React from 'react';
import MuiDivider from '@mui/material/Divider';

const Divider = ({
  color,
  light = false,
}: IProps) => {
  return (
    <MuiDivider
      color={color}
      light={light}
    />
  );
};

interface IProps {
  color?: string,
  light?: boolean,
}

export default Divider;
