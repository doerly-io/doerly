import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const Edit = ({
  color = 'page',
  size = 32,
}: IProps) => {
  const { theme } = useTheme();
  const actualColor = theme.icon.color[color] || color;
  return (
    <SvgIcon
      style={{
        height: `${size}px`,
        width: `${size}px`,
      }}
      viewBox="0 0 24 24"
    >
      <path
        d="M 3 17.25 V 21 h 3.75 L 17.81 9.94 l -3.75 -3.75 L 3 17.25 Z M 20.71 7.04 c 0.39 -0.39 0.39 -1.02 0 -1.41 l -2.34 -2.34 c -0.39 -0.39 -1.02 -0.39 -1.41 0 l -1.83 1.83 l 3.75 3.75 l 1.83 -1.83 Z"
        fill={actualColor}
      />
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string,
  size?: number,
}

export default Edit;
