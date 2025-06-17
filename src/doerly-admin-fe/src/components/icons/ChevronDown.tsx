import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const ChevronDown = ({ color = 'page', size = 24 }: IProps) => {
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
        d="M7.41 8.59 12 13.17l4.59-4.58L18 10l-6 6-6-6z"
        fill={actualColor}
      />
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string;
  size?: number;
}

export default ChevronDown;
