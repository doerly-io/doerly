import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const ChevronRight = ({ color = 'page', size = 24 }: IProps) => {
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
        d="M10 6L8.59 7.41L13.17 12l-4.58 4.59L10 18l6-6z"
        fill={actualColor}
      ></path>
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string;
  size?: number;
}

export default ChevronRight;
